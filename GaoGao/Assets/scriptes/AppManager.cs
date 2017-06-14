using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GaoGao
{
    public class AppManager : MonoBehaviour
    {
        static public AppManager m_singleton;

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
        }

        virtual public void OnDestroy()
        {
            m_singleton = null;
        }
    }
}