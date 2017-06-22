using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GaoGao
{
    [System.Serializable]
    public class ColorSet
    {
        public string   tex;
        public string   icon;
        public int      R,G,B;

        public ColorSet( string t, string i, int r, int g, int b )
        {
            tex = t;
            icon = i;
            R = r;
            G = g;
            B = b;
        }
    };

    public class WheelSet
    {
        public string icon;
        public Transform[]  m_Wheels = new Transform[4];

        public WheelSet( string io, string res, Transform[] parent, bool active )
        {
            icon = io;
            Object resobj = Resources.Load(res);
            for( int i = 0; i < parent.Length; ++i )
            {
                GameObject obj = (GameObject)GameObject.Instantiate(resobj);
                obj.transform.parent = parent[i];
                m_Wheels[i] = obj.transform;
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localScale = i <= 1 ? new Vector3(1,1,1) : new Vector3(-1,1,1);
                obj.SetActive(active);
            }
        }


        public void Enable( bool active )
        {
            for( int i = 0; i < m_Wheels.Length; ++i )
                m_Wheels[i].gameObject.SetActive(active);
        }
    };

    public class CarConfigSet : MonoBehaviour
    {
        public Transform[]      m_Wheels = new Transform[4]; // FL,BL,FR,BR

        public string           m_Icon;

        public List<ColorSet>   m_colors = new List<ColorSet>(10);
        public int              m_curColor = -1;
        
        public List<ColorSet>   m_inside = new List<ColorSet>(10);
        public int              m_curInside = -1;

        public MeshRenderer[]   m_meshes;
        public List<Material>   m_BaseMat = new List<Material>(100);
        public List<Material>   m_InsideMat = new List<Material>(100);

        public List<WheelSet>   m_WheelSets = new List<WheelSet>(10);
        public int              m_curWheel = 0;

        public void OnInit( stCarItem item )
        {
            
            _CollectMaterials();

            m_Icon = item.m_icon;
            for( int i = 0; i < item.m_ColorSets.Length; ++i )
            {
                stColorItem colorItem = TableItemManager<stColorItem>.Instance().GetstItem(item.m_ColorSets[i]);
                if ( colorItem != null )
                {
                    ColorSet co = new ColorSet( colorItem.m_texture, colorItem.m_icon, colorItem.m_R, colorItem.m_G, colorItem.m_B );
                    m_colors.Add(co);
                }
            }

            SwitchColor( 0 );

            for( int i = 0; i < item.m_insideSets.Length; ++i )
            {
                stInsideItem insideItem = TableItemManager<stInsideItem>.Instance().GetstItem(item.m_insideSets[i] );
                if ( insideItem != null)
                {
                    ColorSet ins = new ColorSet( insideItem.m_texture, insideItem.m_icon, insideItem.m_R, insideItem.m_G, insideItem.m_B) ;
                    m_inside.Add(ins);
                }

            }
            SwitchInside(0);

            for ( int i = 0; i < item.m_WheelSets.Length; ++i )
            {
                stWheelItem wItem = TableItemManager<stWheelItem>.Instance().GetstItem(item.m_WheelSets[i]);
                if ( wItem != null )
                {
                    WheelSet ws = new WheelSet( wItem.m_icon, wItem.m_prefab, m_Wheels, i==0? true:false);
                    m_WheelSets.Add(ws);
                }
            }
            m_curWheel = 0;
        }

        void _CollectMaterials()
        {
            m_meshes = GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < m_meshes.Length; ++i)
            {
                Material[] mats = m_meshes[i].materials;
                for (int j = 0; j < mats.Length; ++j)
                {
                    if ( mats[j].name.Contains("BodyPaint"))
                    {
                        m_BaseMat.Add(mats[j]);
                    }
                    if( mats[j].name.Contains("Interior_1") )
                    {
                        m_InsideMat.Add(mats[j]);
                    }
                }
            }
        }

        public void SwitchNextColor()
        {
            int color = m_curColor + 1;
            if (color == m_colors.Count )
                color = 0;
            SwitchColor(color);
        }

        public void SwitchColor( int color )
        {
            if ( m_curColor != color )
            {
                m_curColor = color;
                if (m_curColor < m_colors.Count)
                {
                    ColorSet set = m_colors[m_curColor];
                    Color clr = new Color( set.R/255, set.G/255, set.B/255 );
                    for( int i = 0; i < m_BaseMat.Count; ++i )
                    {
                        m_BaseMat[i].color = clr;
                    }
                }
            }
        }

        public void SwitchNextInside()
        {
            int color = m_curInside + 1;
            if (color == m_inside.Count)
                color = 0;
            SwitchInside(color);
        }

        public void SwitchInside( int inside )
        {
            if (m_curInside != inside)
            {
                m_curInside = inside;
                if (m_curInside < m_inside.Count)
                {
                    ColorSet set = m_inside[m_curInside];
                    Color clr = new Color(set.R / 255, set.G / 255, set.B / 255);
                    for (int i = 0; i < m_InsideMat.Count; ++i)
                    {
                        m_InsideMat[i].color = clr;
                    }
                }
            }
        }

        public void SwitchNextWheel()
        {
            int wID = m_curWheel + 1;
            if (wID == m_WheelSets.Count)
                wID = 0;
            SwitchWheel(wID);
        }

        public void SwitchWheel( int wheel )
        {
            if ( m_curWheel != wheel && wheel < m_WheelSets.Count )
            {
                WheelSet old = m_WheelSets[m_curWheel];
                WheelSet newOne = m_WheelSets[wheel];

                old.Enable(false);
                newOne.Enable(true);
                m_curWheel = wheel;

            }
        }

    }
}