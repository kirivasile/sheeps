using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

namespace Sheeps.GUI {
    public class GuiView : MonoBehaviour {
        [SerializeField] GameObject startMenu;
        [SerializeField] Button newGame;
        [SerializeField] Button continueGame;

        [SerializeField] GameObject parameterMenu;

        [SerializeField] Slider mapSizeSlider;
        Text mapSizeText;

        [SerializeField] Slider numAnimalsSlider;
        Text numAnimalsText;

        [SerializeField] Slider animalSpeedSlider;
        Text animalSpeedText;

        [SerializeField] Button startSimulation;

        [SerializeField] Button saveSimulation;
        [SerializeField] Button pauseSimulation;
        Text pauseButtonText;

        [SerializeField] Text messageText;

        [SerializeField] string mapSizeTemplate = "N = {0}";
        [SerializeField] string numAnimalsTemplate = "M = {0}";
        [SerializeField] string animalSpeedTemplate = "V = {0}";

        private void Start() {
            Assert.IsNotNull(mapSizeSlider);
            mapSizeText = mapSizeSlider.GetComponentInChildren<Text>();
            mapSizeSlider.onValueChanged.AddListener((value) => {
                mapSizeText.text = string.Format(mapSizeTemplate, (int)value);
            });

            Assert.IsNotNull(numAnimalsSlider);
            numAnimalsText = numAnimalsSlider.GetComponentInChildren<Text>();
            numAnimalsSlider.onValueChanged.AddListener((value) => {
                numAnimalsText.text = string.Format(numAnimalsTemplate, (int)value);
            });

            Assert.IsNotNull(animalSpeedSlider);
            animalSpeedText = animalSpeedSlider.GetComponentInChildren<Text>();
            animalSpeedSlider.onValueChanged.AddListener((value) => {
                animalSpeedText.text = string.Format(animalSpeedTemplate, (int)value);
            });

            pauseButtonText = pauseSimulation.GetComponentInChildren<Text>();
        }

        public Button NewGame => newGame;
        public Button ContinueGame => continueGame;
        public Button StartSimulation => startSimulation;
        public Button SaveSimulation => saveSimulation;
        public Button PauseSimulation => pauseSimulation;

        public GameObject StartMenu => startMenu;
        public GameObject ParameterMenu => parameterMenu;

        public Text MessageText => messageText;

        public int MapSize => (int)mapSizeSlider.value;
        public int NumAnimals => (int)numAnimalsSlider.value;
        public int AnimalSpeed => (int)animalSpeedSlider.value;

        public string PauseButtonText {
            get {
                return pauseButtonText.text;
            }
            set {
                pauseButtonText.text = value;
            }
        }

        public Slider MapSizeSlider => mapSizeSlider;
        public Slider NumAnimalsSlider => numAnimalsSlider;
        public Slider AnimalSpeedSlider => animalSpeedSlider;
    }
}