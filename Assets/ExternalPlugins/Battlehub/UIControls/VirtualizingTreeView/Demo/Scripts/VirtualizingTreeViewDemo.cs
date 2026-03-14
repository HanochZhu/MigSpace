using Mig;
using Mig.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.UIControls
{
    /// <summary>
    /// In this demo we use game objects hierarchy as data source (each data item is game object)
    /// You can use any hierarchical data with treeview.
    /// </summary>
    public class VirtualizingTreeViewDemo : MonoBehaviour
    {
        public VirtualizingTreeView TreeView;

        private void OnEnable()
        {
            ModelManager.Instance.OnModelLoadCompleteEvent += RefreshDataItems;
            EventManager.StartListening(Events.OnDeleteModel, OnModelDestroy);
        }
        private void OnDisable()
        {
            if (ModelManager.Instance)
            {
                ModelManager.Instance.OnModelLoadCompleteEvent -= RefreshDataItems;
            }
            EventManager.StopListening(Events.OnDeleteModel, OnModelDestroy);
        }

        public static bool IsPrefab(Transform This)
        {
            if (Application.isEditor && !Application.isPlaying)
            {
                throw new InvalidOperationException("Does not work in edit mode");
            }
            return This.gameObject.scene.buildIndex < 0;
        }


        private void OnItemBeginDrop(object sender, ItemDropCancelArgs e)
        {
            //object dropTarget = e.DropTarget;
            //if(e.Action == ItemDropAction.SetNextSibling || e.Action == ItemDropAction.SetPrevSibling)
            //{
            //    e.Cancel = true;
            //}

        }

        private void Start()
        {
            TreeView.ItemDataBinding += OnItemDataBinding;
            TreeView.SelectionChanged += OnSelectionChanged;
            TreeView.ItemsRemoved += OnItemsRemoved;
            TreeView.ItemExpanding += OnItemExpanding;
            TreeView.ItemBeginDrag += OnItemBeginDrag;

            TreeView.ItemDrop += OnItemDrop;
            TreeView.ItemBeginDrop += OnItemBeginDrop;
            TreeView.ItemEndDrag += OnItemEndDrag;

            //var items = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        }
        private void RefreshDataItems()
        {
            GameObject item = (GameObject)ModelManager.Instance.CurrentSelectGameObject;
            if (item != null)
            {
                var rootChildren = new List<GameObject>();

                // 遍历 "fbx" 的所有子对象
                for (int i = 0; i < item.transform.childCount; i++)
                {
                    var child = item.transform.GetChild(i).gameObject;

                    // 仅添加没有父对象的子对象
                    if (child.transform.parent == item.transform)
                    {
                        rootChildren.Add(child);
                    }
                }

                // 将子对象排序
                var newItems = rootChildren.OrderBy(t => t.transform.GetSiblingIndex()).ToList();
                // 更新树视图的数据项
                TreeView.Items = newItems;
            }
            else
            {
                Debug.Log("GameObject 'fbx' not found");
            }
        }

        private void OnDestroy()
        {
            TreeView.ItemDataBinding -= OnItemDataBinding;
            TreeView.SelectionChanged -= OnSelectionChanged;
            TreeView.ItemsRemoved -= OnItemsRemoved;
            TreeView.ItemExpanding -= OnItemExpanding;
            TreeView.ItemBeginDrag -= OnItemBeginDrag;
            TreeView.ItemBeginDrop -= OnItemBeginDrop;
            TreeView.ItemDrop -= OnItemDrop;
            TreeView.ItemEndDrag -= OnItemEndDrag;
        }

        private void OnItemExpanding(object sender, VirtualizingItemExpandingArgs e)
        {
            //get parent data item (game object in our case)
            GameObject gameObject = (GameObject)e.Item;
            if(gameObject.transform.childCount > 0)
            {
                //get children
                List<GameObject> children = new List<GameObject>();

                for (int i = 0; i < gameObject.transform.childCount; ++i)
                {
                    GameObject child = gameObject.transform.GetChild(i).gameObject;

                    children.Add(child);
                }
                
                //Populate children collection
                e.Children = children;
            }
        }

        private void OnSelectionChanged(object sender, SelectionChangedArgs e)
        {
            #if UNITY_EDITOR
            //Do something on selection changed (just syncronized with editor's hierarchy for demo purposes)
            UnityEditor.Selection.objects = e.NewItems.OfType<GameObject>().ToArray();
            #endif
        }
        private void OnModelDestroy(object obj1, object obj2)
        {
            TreeView.Items = null;
        }
        private void OnItemsRemoved(object sender, ItemsRemovedArgs e)
        {
            //Destroy removed dataitems
            for (int i = 0; i < e.Items.Length; ++i)
            {
                GameObject go = (GameObject)e.Items[i];
                if(go != null)
                {
                    Destroy(go);
                }
            }
        }

        /// <summary>
        /// This method called for each data item during databinding operation
        /// You have to bind data item properties to ui elements in order to display them.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnItemDataBinding(object sender, VirtualizingTreeViewItemDataBindingArgs e)
        {
            GameObject dataItem = e.Item as GameObject;
            Debug.Log("dataItem" + dataItem);
            if (dataItem != null)
            {   
                //We display dataItem.name using UI.Text 
                Text text = e.ItemPresenter.GetComponentInChildren<Text>(true);
                text.text = dataItem.name;

                //Load icon from resources
                Image icon = e.ItemPresenter.GetComponentsInChildren<Image>()[4];
                icon.sprite = Resources.Load<Sprite>("cube");

                Button button = e.ItemPresenter.transform.Find("eye").GetComponent<Button>();
                
                button.onClick.AddListener(()=> {
                    OnItemActive(dataItem);
                });
                //And specify whether data item has children (to display expander arrow if needed)

                e.HasChildren = dataItem.transform.childCount > 0;
                
            }
        }

        private void OnItemBeginDrag(object sender, ItemArgs e)
        {
            //Could be used to change cursor
        }

        private void OnItemDrop(object sender, ItemDropArgs e)
        {
            if(e.DropTarget == null)
            {
                return;
            }

            Transform dropT = ((GameObject)e.DropTarget).transform;
            
            //Set drag items as children of drop target
            if (e.Action == ItemDropAction.SetLastChild)
            {
                for (int i = 0; i < e.DragItems.Length; ++i)
                {
                    Transform dragT = ((GameObject)e.DragItems[i]).transform;
                    dragT.SetParent(dropT, true);
                    dragT.SetAsLastSibling();
                }
            }

            //Put drag items next to drop target
            else if (e.Action == ItemDropAction.SetNextSibling)
            {
                for (int i = e.DragItems.Length - 1; i >= 0; --i)
                {
                    Transform dragT = ((GameObject)e.DragItems[i]).transform;
                    int dropTIndex = dropT.GetSiblingIndex();
                    if (dragT.parent != dropT.parent)
                    {
                        dragT.SetParent(dropT.parent, true);
                        dragT.SetSiblingIndex(dropTIndex + 1);
                    }
                    else
                    {
                        int dragTIndex = dragT.GetSiblingIndex();
                        if (dropTIndex < dragTIndex)
                        {
                            dragT.SetSiblingIndex(dropTIndex + 1);
                        }
                        else
                        {
                            dragT.SetSiblingIndex(dropTIndex);
                        }
                    } 
                }
            }

            //Put drag items before drop target
            else if (e.Action == ItemDropAction.SetPrevSibling)
            {
                for (int i = 0; i < e.DragItems.Length; ++i)
                {
                    Transform dragT = ((GameObject)e.DragItems[i]).transform;
                    if (dragT.parent != dropT.parent)
                    {
                        dragT.SetParent(dropT.parent, true);
                    }

                    int dropTIndex = dropT.GetSiblingIndex();
                    int dragTIndex = dragT.GetSiblingIndex();
                    if(dropTIndex > dragTIndex)
                    {
                        dragT.SetSiblingIndex(dropTIndex - 1);
                    }
                    else
                    {
                        dragT.SetSiblingIndex(dropTIndex);
                    }
                }
            }
        }

        private void OnItemEndDrag(object sender, ItemArgs e)
        {            
        }

        public void OnItemActive(GameObject obj) 
        {
            TreeView.OnTreeListItemActive(obj);
        }
    }
}
