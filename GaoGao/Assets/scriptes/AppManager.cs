﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GaoGao
{
    public class AppManager : MonoBehaviour
    {
        static public AppManager m_singleton;

        public List<CarConfigSet>   m_carLists = new List<CarConfigSet>(5);
        public int                  m_curCarIndex = 0;
        public int                  m_curSceneIndex = 0;
        public Transform            m_OutCamera;
        public Transform            m_InCamera;

        public static AppManager Singleton
        {
            get
            {
                return m_singleton;
            }
            private set { }
        }
        virtual protected void Awake()
        {
            if (m_singleton == null)
            {
                m_singleton = GetComponent<AppManager>();

            }
            DontDestroyOnLoad(gameObject);
        }

        public void Start()
        {
            // load default car
            TableManager.InitLoading();

            int count = TableItemManager<stCarItem>.Instance().GetstItemCount();
            for( int i = 0; i < count; ++i )
            {
                int key = 0;
                stCarItem item = TableItemManager<stCarItem>.Instance().GetstItemByIndex( i, out key );
                if ( item != null )
                {
                    GameObject obj = (GameObject)Instantiate( Resources.Load(item.m_PrefabName, typeof(GameObject) ));
                    obj.transform.parent = transform;
                    CarConfigSet cs = obj.GetComponent<CarConfigSet>();
                    m_carLists.Add(cs);
                    cs.OnInit(item);
                    if ( i > 0 )
                        obj.SetActive(false);
                }
            }

            m_curCarIndex = 0;

            int scenecount = TableItemManager<stSceneItem>.Instance().GetstItemCount();
            //for( int i = 0; i < scenecount; ++i )
            m_curSceneIndex = 0;
            {
                int key = 0;
                stSceneItem item = TableItemManager<stSceneItem>.Instance().GetstItemByIndex(m_curSceneIndex, out key );
                if ( item != null)
                    SceneManager.LoadSceneAsync(item.m_mapName);
            }
           
        }
        virtual public void OnDestroy()
        {
            TableManager.ClearupTable();
            m_singleton = null;
        }

        public void SwitchCar()
        {
            CarConfigSet old = m_carLists[m_curCarIndex];
            ++m_curCarIndex;
            if ( m_curCarIndex == m_carLists.Count )
                m_curCarIndex = 0;
            CarConfigSet newOne = m_carLists[m_curCarIndex];

            old.gameObject.SetActive(false);
            newOne.gameObject.SetActive(true);
        }

        public void SwitchColor()
        {
            m_carLists[m_curCarIndex].SwitchNextColor();
        }

        public void SwitchInside()
        {
            m_carLists[m_curCarIndex].SwitchNextInside();
        }

        public void SwitchWheel()
        {
            m_carLists[m_curCarIndex].SwitchNextWheel();
        }

        public void SwitchCamera()
        {
            m_OutCamera.gameObject.SetActive( ! m_OutCamera.gameObject.activeSelf );
            m_InCamera.gameObject.SetActive( ! m_OutCamera.gameObject.activeSelf );
        }

        public void SwitchScene()
        {
            int scenecount = TableItemManager<stSceneItem>.Instance().GetstItemCount();
            ++m_curSceneIndex;
            if ( m_curSceneIndex == scenecount )
            {
                m_curSceneIndex = 0;
            }
       
            int key = 0;
            stSceneItem item = TableItemManager<stSceneItem>.Instance().GetstItemByIndex(m_curSceneIndex, out key);
            if (item != null)
                SceneManager.LoadScene(item.m_mapName);
        }
    }
}