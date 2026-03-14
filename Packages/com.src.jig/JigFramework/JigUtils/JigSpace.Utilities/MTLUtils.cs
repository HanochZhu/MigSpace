using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace JigSpace.Utilities
{
	public static class MTLUtils
	{
		public class MTLMaterial
		{
			public string Name;

			public Color? Diffuse;
		}

		public static List<MTLMaterial> FromMTLStringToMTLMaterialList(string mtlString)
		{
			List<MTLMaterial> list = new List<MTLMaterial>();
			MTLMaterial mTLMaterial = new MTLMaterial();
			bool flag = true;
			Color color = default(Color);
			color.a = 1f;
			Color color2 = color;
			if (mtlString == null)
			{
				mtlString = "";
			}
			mtlString += "\n";
			for (int i = 0; i < mtlString.Length; i++)
			{
				char c = mtlString[i];
				if (c == '\n')
				{
					flag = true;
				}
				else if (flag && c == 'n' && i < mtlString.Length - 7 && mtlString.Substring(i, 7) == "newmtl ")
				{
					i += 7;
					int num = mtlString.IndexOfEndOfLine(i);
					if (num > i)
					{
						if (!string.IsNullOrEmpty(mTLMaterial.Name))
						{
							list.Add(mTLMaterial);
							mTLMaterial = new MTLMaterial
							{
								Diffuse = Color.white
							};
						}
						mTLMaterial.Name = mtlString.Substring(i, num - i).Trim();
						i = num - 1;
					}
				}
				else if (flag && c == 'K' && i < mtlString.Length - 3 && mtlString.Substring(i, 3) == "Kd ")
				{
					i += 2;
					int num2 = mtlString.IndexOfEndOfLine(i);
					if (num2 > i)
					{
						color2 = GetColorFromObjString(mtlString.Substring(i, num2 - i).Trim());
						mTLMaterial.Diffuse = color2;
						i = num2 - 1;
					}
				}
				else if (flag && c == 'K' && i < mtlString.Length - 3 && mtlString.Substring(i, 3) == "Ks ")
				{
					i += 2;
					int num3 = mtlString.IndexOfEndOfLine(i);
					if (num3 > i)
					{
						i = num3 - 1;
					}
				}
				else if (flag && c == 'd' && i < mtlString.Length - 2 && mtlString[i + 1] == ' ')
				{
					i += 2;
					int num4 = mtlString.IndexOfEndOfLine(i);
					if (num4 > i)
					{
						float a = mtlString.Substring(i, num4 - i).Trim().ToFloat();
						Color value = Color.white;
						if (mTLMaterial.Diffuse.HasValue)
						{
							value = mTLMaterial.Diffuse.Value;
						}
						value.a = a;
						mTLMaterial.Diffuse = value;
						i = num4 - 1;
					}
				}
				else if (flag && c == 'T' && i < mtlString.Length - 3 && mtlString.Substring(i, 3) == "Tr ")
				{
					i += 3;
					int num5 = mtlString.IndexOfEndOfLine(i);
					if (num5 > i)
					{
						i = num5 - 1;
					}
				}
				else if (flag && c == 'N' && i < mtlString.Length - 3 && mtlString.Substring(i, 3) == "Ns ")
				{
					i += 3;
					int num6 = mtlString.IndexOfEndOfLine(i);
					if (num6 > i)
					{
						i = num6 - 1;
					}
				}
				else if (flag && c == 'm' && i < mtlString.Length - 7 && mtlString.Substring(i, 7) == "map_Kd ")
				{
					i += 7;
					int num7 = mtlString.IndexOfEndOfLine(i);
					if (num7 > i)
					{
						i = num7 - 1;
					}
				}
				else if (flag && c == 'm' && i < mtlString.Length - 6 && mtlString.Substring(i, 7) == "map_d ")
				{
					i += 6;
					int num8 = mtlString.IndexOfEndOfLine(i);
					if (num8 > i)
					{
						i = num8 - 1;
					}
				}
				if (c != ' ' && c != '\r' && c != '\n' && c != '\t')
				{
					flag = false;
				}
			}
			if (!string.IsNullOrEmpty(mTLMaterial.Name))
			{
				list.Add(mTLMaterial);
			}
			return list;
		}

		private static Color GetColorFromObjString(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return default(Color);
			}
			string[] array = str.Split(' ', StringSplitOptions.None);
			if (array != null && array.Length == 3)
			{
				float result;
				float.TryParse(array[0], NumberStyles.Float, CultureInfo.InvariantCulture, out result);
				float result2;
				float.TryParse(array[1], NumberStyles.Float, CultureInfo.InvariantCulture, out result2);
				float result3;
				float.TryParse(array[2], NumberStyles.Float, CultureInfo.InvariantCulture, out result3);
				return new Color(result, result2, result3);
			}
			return default(Color);
		}
	}
}
