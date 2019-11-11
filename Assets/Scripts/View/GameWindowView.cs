using System;
using DG.Tweening;
using Slice;
using Source.Pool.PoolObjects;
using Source.Resource;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace View
{
    public class GameWindowView : MonoBehaviour
    {
        private IResourceProvider _resourceProvider;

        [SerializeField] private Image bankImage;
        [SerializeField] private SliceDisplay bankDisplayer;
        [SerializeField] private Button[] receiverBtn;
        [SerializeField] private SliceDisplay[] receiverDisplayers;

        public event Action<int> ReceiverClickEvent;

        [Inject]
        public void Constructor(IResourceProvider resourceProvider)
        {
            _resourceProvider = resourceProvider;
            bankDisplayer.SetResourceProvider(_resourceProvider);

            for (var i = 0; i < receiverBtn.Length; i++)
            {
                var temp = i;
                receiverBtn[i].onClick.AddListener((() => ReceiverClickEvent?.Invoke(temp)));
                receiverDisplayers[i].SetResourceProvider(_resourceProvider);
            }
        }

        public void DrawSliceOnBank(SliceSet sliceSet)
        {
            bankDisplayer.DrawSlice(sliceSet);
        }

        public void DrawSliceOnReceiver(int index, SliceSet sliceSet)
        {
            receiverDisplayers[index].DrawSlice(sliceSet);
        }

        public void ClearBank()
        {
            bankDisplayer.Clear();
        }

        public void ClearReceiver(int index)
        {
            receiverDisplayers[index].Clear();
        }

        public void ShowDestroyParticle(int index)
        {
            _resourceProvider.Get<DestroyParticle>(this.receiverBtn[index].transform);
        }

        public void ErrorAnimation()
        {
            bankImage.DOColor(new Color(1f, 0.6f, 0.6f), 0.14f).SetEase(Ease.InOutBounce).SetLoops(4, LoopType.Yoyo)
                .OnComplete(() => { bankImage.color = Color.white; });
        }

        public void SliceMoveAnimation(int index, Action onComplete)
        {
            bankDisplayer.transform.DOMove(receiverBtn[index].transform.position, 0.2f).SetEase(Ease.OutQuad)
                .OnComplete(
                    () =>
                    {
                        onComplete?.Invoke();
                        bankDisplayer.transform.localPosition = Vector3.zero;
                    });
        }
    }
}