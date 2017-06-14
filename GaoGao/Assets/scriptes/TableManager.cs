using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ WUKAI ADD: loading runtime table ]
namespace GaoGao
{
    public class TableItemManager<T>    :   BaseDataManager where T : BaseData, new()
    {
        private static TableItemManager<T>  s_Instance;

        public static TableItemManager<T>   Instance()
        {
            if ( s_Instance == null)
                s_Instance = new TableItemManager<T>();
            return s_Instance;
        }

        protected override BaseData NewItem()
        {
            return new T();
        }

        public T GetstItem(int nID)
        {
            if (m_DataMap.ContainsKey(nID))
                return (T)m_DataMap[nID];

            return null;
        }

        public int GetstItemCount()
        {
            return m_DataMap.Count;
        }

        public T GetstItemByIndex(int index, out int key)
        {
            int i = 0;
            foreach (KeyValuePair<int, BaseData> pair in m_DataMap)
            {
                if (i == index)
                {
                    key = pair.Key;
                    return (T)pair.Value;
                }
                i++;
            }

            // wrong index
            key = -1;
            return null;
            //			BaseData data = m_DataMap.ElementAt (index).Value;
            //			key = m_DataMap.ElementAt (index).Key;
            //			return (T)data;
        }

        public void Push(T item)
        {
            if (item != null)
                m_DataMap[item.m_nId] = item;
        }

        public void ClearUp()
        {
            s_Instance = null;
        }
    }


    class TableManager 
    {
        public static void InitLoading()
        {
            TableItemManager<stCarItem>.Instance().LoadFile("CarConfig", "Table/");
            TableItemManager<stColorItem>.Instance().LoadFile("Colors", "Table/");
            TableItemManager<stWheelItem>.Instance().LoadFile("Wheels", "Table/");

        }


        public static void ClearupTable()
        {
            TableItemManager<stCarItem>.Instance().ClearUp();
            TableItemManager<stColorItem>.Instance().ClearUp();
            TableItemManager<stWheelItem>.Instance().ClearUp();
        }
    }
}
