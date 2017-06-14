using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
using UnityEngine;

namespace GaoGao
{
	public class TableFile
	{
		enum FIELD_TYPE
		{
			T_INT = 0,		//Integer
			T_FLOAT = 1,	//Float
			T_STRING = 2,	//string
		};       

		ArrayList m_DataTypeArray;   	
		ArrayList m_DataList;         	
		ArrayList m_RowListNow;       	

		public TableFile()
		{
			m_DataTypeArray = new ArrayList(16);
			m_DataList = new ArrayList(16);
		}

		public bool LoadFile(string filename, string filePath)
		{
			TextAsset ptext = (TextAsset)Resources.Load(filePath + filename, typeof(TextAsset));

			if (ptext == null || ptext.text=="")
			{
				return false;
			}
			string ss = ptext.text.ToString().Trim();
			int index = 0;
			int line = 1;
			int end = 0;
			string tempstr = string.Empty;

			while (end > -1)
			{
				end = ss.IndexOf("\r\n", index);
				if (end == -1)
				{
					tempstr = ss.Substring(index, ss.Length - index);
				}
				else
				{
					tempstr = ss.Substring(index, end - index);
				}

				ProxyStr(tempstr, line);
				line++;

				index = end + 1;
			}
			return true;
		}

		void ProxyStr(string str, int line)
		{
			int index = 0;
			int end = 0;
			int dataNum = 0;

			if (line == 1)
			{
				while (end > -1)
				{
					end = str.IndexOf("\t", index);
					string tempStr = string.Empty;
					if (end > -1)
					{
						tempStr = str.Substring(index, end - index);
					}
					else
					{
						tempStr = str.Substring(index, str.Length - index);
					}
					index = end + 1;

					FIELD_TYPE ptype = FIELD_TYPE.T_INT;
					if (tempStr == "INT")
					{
						ptype = FIELD_TYPE.T_INT;
					}
					else if (tempStr == "FLOAT")
					{
						ptype = FIELD_TYPE.T_FLOAT;
					}
					else if (tempStr == "STRING")
					{
						ptype = FIELD_TYPE.T_STRING;
					}
					m_DataTypeArray.Add(ptype);                    
				}
			}
			else if (line > 2)
			{
				ArrayList mRowDataList = new ArrayList(16);  
				m_DataList.Add(mRowDataList);
				dataNum =0;
				while (dataNum < m_DataTypeArray.Count)
				{
					end = str.IndexOf("\t", index);
					string tempStr = string.Empty;
					if (end > -1)
					{
						tempStr = str.Substring(index, end - index);
					}
					else
					{
						if (index < str.Length)
						{
							tempStr = str.Substring(index, str.Length - index);
						} 
						else
						{
							tempStr = string.Empty;
						}
						end = str.Length - 1;
					}
					index = end + 1;                    

					FIELD_TYPE ptype = (FIELD_TYPE)m_DataTypeArray[dataNum];
					dataNum++;

					if (ptype == FIELD_TYPE.T_INT)
					{                       
						if (tempStr == "")
						{
							int tempInt = 0;
							mRowDataList.Add(tempInt);
						}
						else
						{
							int tempInt = Convert.ToInt32(tempStr);
							mRowDataList.Add(tempInt);
						}                        
					}
					else if (ptype == FIELD_TYPE.T_FLOAT)
					{
						if (tempStr == "")
						{
							float tempFloat = 0;
							mRowDataList.Add(tempFloat);
						}
						else
						{
							float tempFloat = (float)Convert.ToDouble(tempStr);
							mRowDataList.Add(tempFloat);
						}                        
					}
					else if (ptype == FIELD_TYPE.T_STRING)
					{                        
						mRowDataList.Add(tempStr);
					}
				}
			}
		}
		public int getRowNum()
		{
			return m_DataList.Count;
		}
		public void SetNowRowList(int col)
		{
			if (col < m_DataList.Count)
			{
				m_RowListNow = (ArrayList)m_DataList[col];
			}
		}
		public int getInt(int col)
		{
			FIELD_TYPE ptype = (FIELD_TYPE)m_DataTypeArray[col];

			if (ptype != FIELD_TYPE.T_INT)
			{
				return -1;
			}

			return (int)m_RowListNow[col];
		}
		public float getFloat(int col)
		{
			FIELD_TYPE ptype = (FIELD_TYPE)m_DataTypeArray[col];

			if (ptype != FIELD_TYPE.T_FLOAT)
			{
				return -1;
			}

			return (float)m_RowListNow[col];
		}
		public string getString(int col)
		{
			FIELD_TYPE ptype = (FIELD_TYPE)m_DataTypeArray[col];

			if (ptype != FIELD_TYPE.T_STRING)
			{
				return null;
			}
			return (string)m_RowListNow[col];
			//            return AnsiToUnicode((string)m_RowListNow[col]);
		}


		public string AnsiToUnicode(string ansiText) 
		{ 
			byte[] gb = System.Text.Encoding.GetEncoding("GB2312").GetBytes(ansiText); 
			string un = System.Text.Encoding.GetEncoding("Unicode").GetString(gb); 
			return un;
		}     
		public string UnicodeToAnsi(string unText)
		{
			byte[] un = System.Text.Encoding.GetEncoding("Unicode").GetBytes(unText);
			string gb = System.Text.Encoding.GetEncoding("GB2312").GetString(un);
			return gb; 
		}
	}


	public class BaseData
	{
		public int m_nId;
		public virtual void LoadData(int nRowIndex,TableFile fileData)
		{

		}
	}

    public interface ISingletonManager
    {
        void DoDestroy();
    }

    public abstract class BaseDataManager   : ISingletonManager
    {
		protected Dictionary<int, BaseData> m_DataMap;
		protected int m_RowNum;
		public BaseDataManager()
		{
			m_DataMap = new Dictionary<int, BaseData>(64);
		}
		public int getRowNum()
		{
			return m_RowNum;
		}
		public void LoadFile(string fileName, string filePath)
		{
			TableFile filedata = new TableFile();
			if (filedata.LoadFile(fileName, filePath)==false)
			{
				StringBuilder sb = new StringBuilder ();
				sb.AppendFormat("Load {0} Error,Please check file", fileName);
				Debug.LogError (sb.ToString ());
				return;				
			}

			m_RowNum = filedata.getRowNum();

			for (int i = 0; i < m_RowNum; i++)
			{
				BaseData item = NewItem();
				filedata.SetNowRowList(i);
				try
				{
					item.LoadData(i, filedata);
					m_DataMap[item.m_nId] = item;
				}
				catch (System.Exception)
				{
					StringBuilder sb = new StringBuilder ();
					sb.AppendFormat("Load File {0}{1} Error: row={2}", filePath, fileName, i);
					Debug.LogError (sb.ToString ());					
					return;
				}

			}
		}
		protected abstract BaseData NewItem();
		public virtual void DoDestroy() 
		{

		}
	}
}
