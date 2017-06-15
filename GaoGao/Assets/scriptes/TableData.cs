using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GaoGao
{
    public class stCarItem : BaseData
    {
        public string   m_PrefabName;
        public string   m_icon;
        public int[]    m_ColorSets;
        public int[]    m_WheelSets;
        public int[]    m_insideSets;


        public override void LoadData(int nRowIndex, TableFile fileData)
        {
            int readIndex = 0;
            m_nId = fileData.getInt(readIndex); readIndex++;
            m_PrefabName = fileData.getString(readIndex); readIndex++;
            m_icon = fileData.getString(readIndex); readIndex++;
            string colorstring = fileData.getString(readIndex); readIndex++;
            string wheelstring = fileData.getString(readIndex); readIndex++;
            string insidestring = fileData.getString(readIndex); readIndex++;

            colorstring = colorstring.Replace(" ", "");
            colorstring = colorstring.Replace("\"", "");
            string[] colorItems = colorstring.Split(',');
            m_ColorSets = new int[colorItems.Length];
            for( int i = 0; i < m_ColorSets.Length; ++i )
            {
                System.Int32.TryParse( colorItems[i], out m_ColorSets[i] );
            }

            wheelstring = wheelstring.Replace(" ", "");
            wheelstring = wheelstring.Replace("\"", "");
            string[] wheelItems = wheelstring.Split(',');
            m_WheelSets = new int[wheelItems.Length];
            for (int i = 0; i < m_WheelSets.Length; ++i)
            {
                System.Int32.TryParse(wheelItems[i], out m_WheelSets[i]);
            }

            insidestring = insidestring.Replace(" ", "");
            insidestring = insidestring.Replace("\"", "");
            string[] insideItems = insidestring.Split(',');
            m_insideSets = new int[insideItems.Length];
            for (int i = 0; i < insideItems.Length; ++i)
            {
                System.Int32.TryParse(insideItems[i], out m_insideSets[i]);
            }

        }
    }

    public class stColorItem : BaseData
    {
        public string   m_texture;
        public string   m_icon;
        public int      m_R;
        public int      m_G;
        public int      m_B;

        public override void LoadData(int nRowIndex, TableFile fileData)
        {
            int readIndex = 0;
            m_nId = fileData.getInt(readIndex); readIndex++;
            m_texture = fileData.getString(readIndex); readIndex++;
            m_icon = fileData.getString(readIndex); readIndex++;
            m_R = fileData.getInt(readIndex); readIndex++;
            m_G = fileData.getInt(readIndex); readIndex++;
            m_B = fileData.getInt(readIndex); readIndex++;
        }
    }

    public class stWheelItem : BaseData
    {
        public string   m_prefab;
        public string   m_icon;

        public override void LoadData(int nRowIndex, TableFile fileData)
        {
            int readIndex = 0;
            m_nId = fileData.getInt(readIndex); readIndex++;
            m_prefab = fileData.getString(readIndex); readIndex++;
            m_icon = fileData.getString(readIndex); readIndex++;
        }

    }

    public class stInsideItem : BaseData
    {
        public string m_texture;
        public string m_icon;
        public int m_R;
        public int m_G;
        public int m_B;

        public override void LoadData(int nRowIndex, TableFile fileData)
        {
            int readIndex = 0;
            m_nId = fileData.getInt(readIndex); readIndex++;
            m_texture = fileData.getString(readIndex); readIndex++;
            m_icon = fileData.getString(readIndex); readIndex++;
            m_R = fileData.getInt(readIndex); readIndex++;
            m_G = fileData.getInt(readIndex); readIndex++;
            m_B = fileData.getInt(readIndex); readIndex++;
        }
    }
}