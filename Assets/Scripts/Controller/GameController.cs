using System.Linq;
using Data;
using Slice;
using Source.DataProvider.Interfaces;
using UnityEngine;
using View;

namespace Controller
{
    public class GameController
    {
        private const int SLICE_MAX = 6;
        private const int REWARD = 25;

        private readonly GameWindowView _gameWindowView;
        private readonly LoseWindowView _loseWindowView;
        private readonly ScorePanelView _scorePanelView;
        private readonly ILocalDataProvider _localDataProvider;

        private SliceSet _bank;
        private SliceSet[] _receivers = new SliceSet[SLICE_MAX];
        //List of probabilities for creating a certain number of pieces
        private readonly int[] _randomProbability;

        private int _totalScore;
        private int _curScore;


        public GameController(GameWindowView gameWindowView,
            LoseWindowView loseWindowView,
            ScorePanelView scorePanelView,
            RandomFrequencyDto randomFrequencyDto, ILocalDataProvider localDataProvider)
        {
            _gameWindowView = gameWindowView;
            _loseWindowView = loseWindowView;
            _scorePanelView = scorePanelView;
            _randomProbability = randomFrequencyDto.frequencyArray;
            _localDataProvider = localDataProvider;

            //Load total score from local file
            if (_localDataProvider.Exist<ScoreDto>())
                _totalScore = _localDataProvider.Load<ScoreDto>().totalScore;
            _scorePanelView.SetTotalScore(_totalScore);

            loseWindowView.RestartBtnClick += ResetGame;
            _gameWindowView.ReceiverClickEvent += ReceiverClickHandle;
            CreateNewSliceSet();
        }

        private void ResetGame()
        {
            _loseWindowView.Hide();
            _gameWindowView.ClearBank();
            _bank = new SliceSet(SliceType.None);
            _scorePanelView.SetCurrentScore(_curScore);

            for (var i = 0; i < _receivers.Length; i++)
            {
                _gameWindowView.ClearReceiver(i);
                _receivers[i] = new SliceSet(SliceType.None);
            }

            CreateNewSliceSet();
        }

        private void ReceiverClickHandle(int index)
        {
            if (!_receivers[index].CanAdd(_bank))
            {
                //Error animation
                _gameWindowView.ErrorAnimation();
                return;
            }

            _receivers[index].Add(_bank);
            _bank.Value = SliceType.None;
            _gameWindowView.SliceMoveAnimation(index, AnimationEndAction);

            //Waiting for the end of the animation to continue execution
            void AnimationEndAction()
            {
                _gameWindowView.DrawSliceOnReceiver(index, _receivers[index]);
                _gameWindowView.ClearBank();
                if (CheckFullSet(index))
                    HandleFullSet(index);

                CreateNewSliceSet();
                if (ValidateLose())
                    HandleLose();
            }
        }

        private void CreateNewSliceSet()
        {
            _bank = GenerateRndSliceSet();
            _gameWindowView.DrawSliceOnBank(_bank);
        }

        private bool ValidateLose()
        {
            return _receivers.All(receiver => !receiver.CanAdd(_bank));
        }

        private void HandleLose()
        {
            _loseWindowView.Show();
        }

        private bool CheckFullSet(int index)
        {
            return _receivers[index].IsFull();
        }

        private void HandleFullSet(int index)
        {
            var reward = 0;
            for (var i = -1; i <= 1; i++)
            {
                var nextIndex = index + i;
                if (nextIndex < 0)
                    nextIndex += (SLICE_MAX);
                if (nextIndex > (SLICE_MAX - 1))
                    nextIndex -= (SLICE_MAX);

                _gameWindowView.ClearReceiver(nextIndex);
                ;
                _gameWindowView.ShowDestroyParticle(nextIndex);
                reward += _receivers[nextIndex].Capacity() * REWARD;
                _receivers[nextIndex].Value = SliceType.None;
            }

            _curScore += reward;
            _totalScore += reward;
            _scorePanelView.SetTotalScore(_totalScore);
            _scorePanelView.SetCurrentScore(_curScore);

            //Saving _totalScore to local file
            var scoreDto = new ScoreDto {totalScore = _totalScore,};
            _localDataProvider.Save(scoreDto);
        }

        private SliceSet GenerateRndSliceSet()
        {
            var amount = 1;
            var max = _randomProbability.Sum();
            var rndAmount = Random.Range(0, max);
            
            //Generate random slice amount via _randomProbability list
            for (var i = 0; i < _randomProbability.Length; i++)
            {
                if (rndAmount < _randomProbability[i])
                {
                    amount = i + 1;
                    break;
                }

                rndAmount -= _randomProbability[i];
            }
            
            //Get random slice index
            var rndNum = Random.Range(0, SLICE_MAX);
            var rndSliceSet = SliceType.None;
            
            //Then we take the following amount of slices
            for (var i = 0; i <= amount - 1; i++)
            {
                var shift = rndNum + i;
                shift = shift >= SLICE_MAX ? shift - SLICE_MAX : shift;
                rndSliceSet |= (SliceType) (1 << shift);
            }

            return new SliceSet(rndSliceSet);
        }
    }
}