using System;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class LoseWindowView : MonoBehaviour
    {
        [SerializeField] private Button restartButton;
        public event Action RestartBtnClick;
        
        private void Start()
        {
            restartButton.onClick.AddListener(() => RestartBtnClick?.Invoke());
        }

        public void Show()
        {
            this.gameObject.SetActive(true);
        }

        public void Hide()
        {
            this.gameObject.SetActive(false);
        }
    }
}