using System;
using System.Collections.Generic;
using System.Linq;
using DoubleDCore.UI.Base;
using UnityEngine;

namespace DoubleDCore.UI
{
    public class UIManager : IUIManager
    {
        private readonly Dictionary<Type, PageBox> _pages = new();

        public event Action<IPage> PageOpened;
        public event Action<IPage> PageClosed;

        public IEnumerable<IPage> ActivePages => _pages
            .Where(p => p.Value.IsOpened)
            .Select(p => p.Value.Page);

        public bool ContainsPage<TPage>() where TPage : class, IPage
        {
            var type = typeof(TPage);

            return _pages.ContainsKey(type);
        }

        public bool ContainsPage(IPage page)
        {
            var type = page.GetType();

            return _pages.ContainsKey(type);
        }

        public void RegisterPage<TPage>(TPage page) where TPage : class, IPage
        {
            var type = typeof(TPage);

            if (ContainsPage<TPage>())
            {
                Debug.LogError($"Attempt to register an existing page {type}");
                return;
            }

            _pages.Add(type, new PageBox
            {
                Page = page,
                IsOpened = false
            });

            page.Initialize();
        }

        public void RegisterPageByType(IPage page)
        {
            var type = page.GetType();

            if (ContainsPage(page))
            {
                Debug.LogError($"Attempt to register an existing page {type}");
                return;
            }

            _pages.Add(type, new PageBox
            {
                Page = page,
                IsOpened = false
            });

            page.Initialize();
        }

        public void RemovePage<TPage>() where TPage : class, IPage
        {
            if (ContainsPage<TPage>() == false)
                return;

            if (PageIsOpened<TPage>())
                ClosePage<TPage>();

            var type = typeof(TPage);
            _pages.Remove(type);
        }

        public void RemovePage(IPage page)
        {
            if (ContainsPage(page) == false)
                return;

            if (PageIsOpened(page))
            {
                page.Close();
                PageClosed?.Invoke(page);
            }

            var type = page.GetType();
            _pages.Remove(type);
        }

        public void OpenPage<TPage>() where TPage : class, IUIPage
        {
            if (ContainsPage<TPage>() == false)
            {
                Debug.LogError($"Page {typeof(TPage).Name} not show. Unregistered page");
                return;
            }

            if (PageIsOpened<TPage>())
            {
                Debug.LogWarning($"Page {typeof(TPage).Name} already open");
                return;
            }
            
            var page = GetPage<TPage>();

            if (page is ISingleOpenablePage)
            {
                foreach (var type in _pages.Keys.ToArray())
                {
                    if (_pages[type].IsOpened && _pages[type].Page is ISingleOpenablePage)
                    {
                        _pages[type].Page.Close();
                        _pages[type].IsOpened = false;
                        PageClosed?.Invoke(_pages[type].Page);
                    }
                } 
            }

            page.Open();
            _pages[typeof(TPage)].IsOpened = true;
            PageOpened?.Invoke(page);
        }

        public void OpenPage<TPage, TPayload>(TPayload context) where TPage : class, IPayloadPage<TPayload>
        {
            if (ContainsPage<TPage>() == false)
            {
                Debug.LogError($"Page {typeof(TPage).Name} not show. Unregistered page");
                return;
            }

            if (PageIsOpened<TPage>())
            {
                Debug.LogWarning($"Page {typeof(TPage).Name} already open");
                return;
            }

            var page = GetPage<TPage>();

            if (page is ISingleOpenablePage)
            {
                foreach (var type in _pages.Keys.ToArray())
                {
                    if (_pages[type].IsOpened && _pages[type].Page is ISingleOpenablePage)
                    {
                        _pages[type].Page.Close();
                        _pages[type].IsOpened = false;
                        PageClosed?.Invoke(_pages[type].Page);
                    }
                } 
            }
            
            page.Open(context);
            _pages[typeof(TPage)].IsOpened = true;
            PageOpened?.Invoke(page);
        }

        public void OpenPage(IUIPage page)
        {
            if (ContainsPage(page) == false)
            {
                Debug.LogError($"Page {page.GetType().Name} not show. Unregistered page");
                return;
            }

            if (PageIsOpened(page))
            {
                Debug.LogWarning($"Page {page.GetType().Name} already open");
                return;
            }
            
            if (page is ISingleOpenablePage)
            {
                foreach (var type in _pages.Keys.ToArray())
                {
                    if (_pages[type].IsOpened && _pages[type].Page is ISingleOpenablePage)
                    {
                        _pages[type].Page.Close();
                        _pages[type].IsOpened = false;
                        PageClosed?.Invoke(_pages[type].Page);
                    }
                } 
            }

            page.Open();
            _pages[page.GetType()].IsOpened = true;
            PageOpened?.Invoke(page);
        }

        public void ClosePage<TPage>() where TPage : class, IPage
        {
            if (PageIsOpened<TPage>() == false)
            {
                Debug.LogWarning($"Page {typeof(TPage).Name} not close. Page is not opened or unregistered");
                return;
            }

            var page = GetPage<TPage>();

            page.Close();
            _pages[typeof(TPage)].IsOpened = false;
            PageClosed?.Invoke(page);
        }
        
        public void ClosePage(IPage page)
        {
            if (PageIsOpened(page) == false)
            {
                Debug.LogWarning($"Page {page.GetType().Name} not close. Page is not opened or unregistered");
                return;
            }
            
            page.Close();
            _pages[page.GetType()].IsOpened = false;
            PageClosed?.Invoke(page);
        }

        public void TogglePage<TPage>() where TPage : class, IUIPage
        {
            if (PageIsOpened<TPage>())
                ClosePage<TPage>();
            else
                OpenPage<TPage>();
        }

        public bool PageIsOpened<TPage>() where TPage : class, IPage
        {
            return ContainsPage<TPage>() && _pages[typeof(TPage)].IsOpened;
        }

        public bool PageIsOpened(IPage page)
        {
            return ContainsPage(page) && _pages[page.GetType()].IsOpened;
        }

        private TPage GetPage<TPage>() where TPage : class, IPage
        {
            var type = typeof(TPage);

            if (ContainsPage<TPage>())
                return _pages[type].Page as TPage;

            Debug.LogError($"Attempt to access an unregistered page {type}");
            return null;
        }

        private class PageBox
        {
            public IPage Page;
            public bool IsOpened;
        }
    }
}