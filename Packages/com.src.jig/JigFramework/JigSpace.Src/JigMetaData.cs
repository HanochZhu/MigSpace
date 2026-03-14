using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

[Serializable]
public class JigMetaData
{
	public static class VisibilityTypes
	{
		public const string Public = "public";

		public const string Link = "link";

		public const string Private = "private";

		public const string Password = "link-password";
	}

	public static class TenantAssociationTypes
	{
		public static readonly string Own = "own";

		public static readonly string Sharing = "sharing";
	}

	[Serializable]
	public class Tenant
	{
		public class Permission
		{
			public const string VIEW = "view";

			public const string SHARABLE_READONLY = "sharable-readonly";
		}

		public int TenantID;

		public List<string> Permissions;

		public string Type;

		public Tenant(int tenantID, List<string> permissions, string associationType)
		{
			TenantID = tenantID;
			Permissions = permissions ?? new List<string>();
			Type = associationType;
		}

		public bool AddPermission(string permission)
		{
			if (Permissions == null)
			{
				Permissions = new List<string>();
			}
			if (!Permissions.Contains(permission))
			{
				Permissions.Add(permission);
				return true;
			}
			return false;
		}

		public bool RemovePermission(string permission)
		{
			if (Permissions != null && Permissions.Contains(permission))
			{
				Permissions.Remove(permission);
				return true;
			}
			return false;
		}

		public bool HasPermission(string permission)
		{
			if (Permissions == null)
			{
				return false;
			}
			return Permissions.Contains(permission);
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			Tenant tenant = obj as Tenant;
			if (tenant == null)
			{
				return false;
			}
			return Equals(tenant);
		}

		public bool Equals(Tenant tenant)
		{
			if (tenant == null)
			{
				return false;
			}
			return TenantID.Equals(tenant.TenantID);
		}

		public override int GetHashCode()
		{
			return TenantID;
		}
	}

	public enum ProState
	{
		Unknown,
		Free,
		Pro
	}

	public enum CopyPermissions
	{
		owner_user,
		owner_tenant,
		anyone
	}

	[NonSerialized]
	public bool IsOldJigNoInnerParents;

	public int Uid;

	public string Id;

	public List<Tenant> Tenants;

	public int Version;

	public string ProjectVisibility;

	public string Type;

	public string Author;

	public string ProjectName;

	public string ProjectDescription;

	public int Views;

	public float FormatVersionAtCreation;

	public float FormatVersion;

	public string DateCreated;

	public string DateUpdated;

	public string DatePublished;

	public string DeeplinkURL;

	public int OwnerTenantID;

	public string ThumbnailURL;

	public bool HasThumbnail;

	public List<string> Tags;

	public List<string> Categories;

	public float GlobalScale = 1f;

	public int DefaultLanguageIndex;

	public string[] LanguageTagsIETF;

	public bool MovoEnabled;

	public string Password;

	[NonSerialized]
	public string OriginalId;

	[NonSerialized]
	private ProState _pro;

	public const string DEFAULT_JIG_NAME = "Untitled";

	[SerializeField]
	private string CopyableBy;

	public ProState Pro => _pro;

	public bool HasCustomName => ProjectName != "Untitled";

	public bool MovoSupported => FormatVersionAtCreation >= 9.2f;

	public bool CanMovo
	{
		get
		{
			return false;
		}
	}

	public CopyPermissions CopyableByPermission
	{
		get
		{
			return GetCopyPermission(CopyableBy);
		}
		set
		{
			CopyableBy = value.ToString();
		}
	}

	public static CopyPermissions GetCopyPermission(string permission)
	{
		switch (permission)
		{
		case "anyone":
			return CopyPermissions.anyone;
		case "owner_tenant":
			return CopyPermissions.owner_tenant;
		default:
			return CopyPermissions.owner_user;
		}
	}

	public JigMetaData()
	{
		Init();
	}

	public JigMetaData(ProState proState)
	{
		Init(proState);
	}

	public Tenant GetTenant(int tenantID, string associationType)
	{
		return Tenants.FirstOrDefault((Tenant x) => x.TenantID == tenantID && x.Type == associationType);
	}

	public Tenant GetOrCreateTenant(int tenantID, string associationType)
	{
		Tenant tenant = Tenants.FirstOrDefault((Tenant x) => x.TenantID == tenantID && x.Type == associationType);
		if (tenant == null)
		{
			tenant = new Tenant(tenantID, null, associationType);
			Tenants.Add(tenant);
		}
		return tenant;
	}

	public bool CanShareJig()
	{
		
		return false;
	}

	private void Init(ProState proState = ProState.Unknown)
	{
		Id = string.Empty;
		SetPro(proState);
		Categories = new List<string>();
		Tags = new List<string>();
		Tenants = new List<Tenant>();
	}

	public void InitForNewJig(ProState proState)
	{
		Init(proState);
		GlobalScale = 1f;
		ProjectDescription = "";
		MovoEnabled = true;
	}

	public void SetPro(ProState proState)
	{
		_pro = proState;
		if (proState == ProState.Pro || string.IsNullOrEmpty(ProjectVisibility))
		{
			ProjectVisibility = "link";
		}
	}

	

	public bool IsNew()
	{
		if (string.IsNullOrEmpty(DatePublished))
		{
			return false;
		}
		if (Convert.ToDateTime(DatePublished).ToLocalTime() > JigSingleton<GameManager>.Instance.FirstRunDate)
		{
			return true;
		}
		return false;
	}

	public bool IsJigOwner()
	{
		{
			return false;
		}
	}

	public bool ThumbnailExists()
	{
		if (!string.IsNullOrEmpty(ThumbnailURL))
		{
			return !ThumbnailURL.Contains("placeholder");
		}
		return false;
	}

}
