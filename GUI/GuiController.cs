using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Sheeps.Core;
using Sheeps.Save;
using Zenject;

namespace Sheeps.GUI {
    public class GuiController : MonoBehaviour {
        GameController _gameController;
        SavingSystem _savingSystem;
        Settings _settings;

        [SerializeField]
        GuiView view;

        bool _pauseButtonClicked;

        [Inject]
        public void Construct(Settings settings, GameController gameController, SavingSystem savingSystem) {
            _gameController = gameController;
            _savingSystem = savingSystem;
            _settings = settings;
        }

        private void Start() {
            InitView();
        }

        void InitView() {
            _pauseButtonClicked = false;
            view.StartMenu.SetActive(true);

            InitButtons();
            InitSliders();
        }

        void InitButtons() {
            view.NewGame.onClick.AddListener(() => {
                view.StartMenu.SetActive(false);
                view.ParameterMenu.SetActive(true);
            });

            view.ContinueGame.onClick.AddListener(() => {
                // TODO: Add states
                view.StartMenu.SetActive(false);
                _gameController.SetSettings(LoadGameSettings());
                if (_savingSystem.Load()) {
                    _gameController.StartSimulation();
                } else {
                    StartCoroutine(ButtonMessage(view.ContinueGame, _settings.saveNotFoundText));
                }
            });

            view.StartSimulation.onClick.AddListener(() => {
                view.ParameterMenu.SetActive(false);
                _gameController.SetSettings(LoadGameSettings());
                _gameController.StartSimulation();
            });

            view.SaveSimulation.onClick.AddListener(() => {
                _savingSystem.Save();
                StartCoroutine(ButtonMessage(view.SaveSimulation, _settings.saveMessageText));
            });

            view.PauseSimulation.onClick.AddListener(() => {
                if (_pauseButtonClicked) {
                    _pauseButtonClicked = false;
                    _gameController.Continue();
                    view.PauseButtonText = _settings.pauseButtonName;
                } else {
                    _pauseButtonClicked = true;
                    _gameController.Pause();
                    view.PauseButtonText = _settings.continueButtonName;
                }
            });
        }

        void InitSliders() {
            view.MapSizeSlider.minValue = _settings.minMapSize;
            view.MapSizeSlider.maxValue = _settings.maxMapSize;

            view.MapSizeSlider.onValueChanged.AddListener((value) => {
                view.NumAnimalsSlider.maxValue = value * value / 4f;
            });

            view.NumAnimalsSlider.minValue = _settings.minNumAnimals;
            view.NumAnimalsSlider.maxValue = _settings.minMapSize * _settings.minMapSize / 2;

            view.AnimalSpeedSlider.minValue = _settings.minSpeed;
            view.AnimalSpeedSlider.maxValue = _settings.maxSpeed;

            view.MapSizeSlider.value = _settings.initialMapSize;
            view.NumAnimalsSlider.value = _settings.initialNumAnimals;
            view.AnimalSpeedSlider.value = _settings.initialSpeed;
        }
        
        IEnumerator ButtonMessage(Button button, string message) {
            view.MessageText.text = message;
            button.interactable = false;
            yield return new WaitForSeconds(_settings.messageDuration);
            button.interactable = true;
            view.MessageText.text = "";
        }

        GameController.Settings LoadGameSettings() {
            return new GameController.Settings() {
                mapSize = view.MapSize,
                numAnimals = view.NumAnimals,
                animalSpeed = (float) view.AnimalSpeed / _settings.speedNormalizer
            };
        }

        [Serializable]
        public class Settings {
            public int minMapSize;
            public int maxMapSize;
            public int minNumAnimals;
            public int minSpeed;
            public int maxSpeed;
            public int speedNormalizer;

            public int initialMapSize;
            public int initialNumAnimals;
            public int initialSpeed;

            public float messageDuration;

            public string pauseButtonName;
            public string continueButtonName;
            public string saveMessageText;
            public string saveNotFoundText;
        }
    }
}