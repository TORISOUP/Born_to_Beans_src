﻿using UnityEngine;

namespace GGJ.Utils
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        protected static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = (T)FindObjectOfType(typeof(T));

                    if (instance == null)
                    {
                       // Debug.LogWarning(typeof(T) + "is nothing");
                    }
                }

                return instance;
            }
        }

        protected void Awake()
        {
            CheckInstance();
        }

        protected bool CheckInstance()
        {
            if (instance == null)
            {
                instance = (T)this;
                return true;
            }
            else if (Instance == this)
            {
                return true;
            }

            Destroy(this);
            return false;
        }
    }
}