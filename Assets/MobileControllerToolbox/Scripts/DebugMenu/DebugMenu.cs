using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


namespace MobileController
{
    public class DebugMenu : MonoBehaviour
    {
        [Header("Behaviour")]
        public float doubleTapTime = 0.3f;
        public bool displayByDefault = false;
        [Space(15)]

        [Header("References")]
        public Transform entryParent;
        public GameObject entryType_1;
        public GameObject entryType_2;
        public GameObject content;


        private List<Analog> analogs;
        private List<Dpad> dpads;
        private List<ArrowKeys> arrowKeys;
        private List<Button> buttons;
        private List<Cooldown> cooldowns;
        private List<Toggle> toggles;
        private List<Swap> swaps;
        private List<TouchDrag> touchDrags;

        private List<TextMeshProUGUI> analogsTxt;
        private List<TextMeshProUGUI> dpadsTxt;
        private List<TextMeshProUGUI> arrowKeysTxt;
        private List<TextMeshProUGUI> buttonsTxt;
        private List<TextMeshProUGUI> cooldownsTxt;
        private List<TextMeshProUGUI> togglesTxt;
        private List<TextMeshProUGUI> swapsTxt;
        private List<TextMeshProUGUI> touchDragsTxt;

        private float lastTapTime;


        /**/


        #region Setup

        private void Start()
        {
            // set display
            content.SetActive(displayByDefault);


            // initialize
            analogs = new List<Analog>();
            analogsTxt = new List<TextMeshProUGUI>();
            analogs = GameObject.FindObjectsOfType<Analog>().ToList();

            dpads = new List<Dpad>();
            dpadsTxt = new List<TextMeshProUGUI>();
            dpads = GameObject.FindObjectsOfType<Dpad>().ToList();

            arrowKeys = new List<ArrowKeys>();
            arrowKeysTxt = new List<TextMeshProUGUI>();
            arrowKeys = GameObject.FindObjectsOfType<ArrowKeys>().ToList();

            buttons = new List<Button>();
            buttonsTxt = new List<TextMeshProUGUI>();
            buttons = GameObject.FindObjectsOfType<Button>().ToList();

            cooldowns = new List<Cooldown>();
            cooldownsTxt = new List<TextMeshProUGUI>();
            cooldowns = GameObject.FindObjectsOfType<Cooldown>().ToList();

            toggles = new List<Toggle>();
            togglesTxt = new List<TextMeshProUGUI>();
            toggles = GameObject.FindObjectsOfType<Toggle>().ToList();

            swaps = new List<Swap>();
            swapsTxt = new List<TextMeshProUGUI>();
            swaps = GameObject.FindObjectsOfType<Swap>().ToList();

            touchDrags = new List<TouchDrag>();
            touchDragsTxt = new List<TextMeshProUGUI>();
            touchDrags = GameObject.FindObjectsOfType<TouchDrag>().ToList();

            // instanciate entries
            InstanciateEntries();
        }


        private void InstanciateEntries()
        {
            bool isType1 = true;
            GameObject tmpObj;

            // analogs
            foreach (Analog item in analogs)
            {
                if (isType1) tmpObj = GameObject.Instantiate(entryType_1, entryParent);
                else tmpObj = GameObject.Instantiate(entryType_2, entryParent);

                analogsTxt.Add(tmpObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>());

                isType1 = !isType1;
            }

            // dpads
            foreach (Dpad item in dpads)
            {
                if (isType1) tmpObj = GameObject.Instantiate(entryType_1, entryParent);
                else tmpObj = GameObject.Instantiate(entryType_2, entryParent);

                dpadsTxt.Add(tmpObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>());

                isType1 = !isType1;
            }

            // arrow keys
            foreach (ArrowKeys item in arrowKeys)
            {
                if (isType1) tmpObj = GameObject.Instantiate(entryType_1, entryParent);
                else tmpObj = GameObject.Instantiate(entryType_2, entryParent);

                arrowKeysTxt.Add(tmpObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>());

                isType1 = !isType1;
            }

            // buttons
            foreach (Button item in buttons)
            {
                if (isType1) tmpObj = GameObject.Instantiate(entryType_1, entryParent);
                else tmpObj = GameObject.Instantiate(entryType_2, entryParent);

                buttonsTxt.Add(tmpObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>());

                isType1 = !isType1;
            }

            // cooldowns
            foreach (Cooldown item in cooldowns)
            {
                if (isType1) tmpObj = GameObject.Instantiate(entryType_1, entryParent);
                else tmpObj = GameObject.Instantiate(entryType_2, entryParent);

                cooldownsTxt.Add(tmpObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>());

                isType1 = !isType1;
            }

            // toggles
            foreach (Toggle item in toggles)
            {
                if (isType1) tmpObj = GameObject.Instantiate(entryType_1, entryParent);
                else tmpObj = GameObject.Instantiate(entryType_2, entryParent);

                togglesTxt.Add(tmpObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>());

                isType1 = !isType1;
            }

            // swaps
            foreach (Swap item in swaps)
            {
                if (isType1) tmpObj = GameObject.Instantiate(entryType_1, entryParent);
                else tmpObj = GameObject.Instantiate(entryType_2, entryParent);

                swapsTxt.Add(tmpObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>());

                isType1 = !isType1;
            }

            // touch Drags
            foreach (TouchDrag item in touchDrags)
            {
                if (isType1) tmpObj = GameObject.Instantiate(entryType_1, entryParent);
                else tmpObj = GameObject.Instantiate(entryType_2, entryParent);

                touchDragsTxt.Add(tmpObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>());

                isType1 = !isType1;
            }
        }

        #endregion


        #region Display

        private void Update()
        {
            // analogs
            for (int i = 0; i < analogsTxt.Count; i++) analogsTxt[i].text = analogs[i].name + ": " + analogs[i].GetAnalogState() + " / " + analogs[i].GetStickPosition();

            // dpads
            for (int i = 0; i < dpadsTxt.Count; i++) dpadsTxt[i].text = dpads[i].name + ": " + dpads[i].GetDpadState() + " / " + dpads[i].GetDpadDirection();

            // arrow keys
            for (int i = 0; i < arrowKeys.Count; i++) arrowKeysTxt[i].text = arrowKeys[i].name + ": " + arrowKeys[i].GetArrowKeysState() + " / " + arrowKeys[i].GetArrowKeysDirection();

            // buttons
            for (int i = 0; i < buttonsTxt.Count; i++) buttonsTxt[i].text = buttons[i].name + ": " + buttons[i].GetButtonState();

            // cooldowns
            for (int i = 0; i < cooldownsTxt.Count; i++) cooldownsTxt[i].text = cooldowns[i].name + ": " + cooldowns[i].GetCooldownState() + " / " + cooldowns[i].GetCooldownTime() + " cooldown";

            // toggles
            for (int i = 0; i < togglesTxt.Count; i++) togglesTxt[i].text = toggles[i].name + ": " + toggles[i].GetToggleState();

            // swaps
            for (int i = 0; i < swapsTxt.Count; i++) swapsTxt[i].text = swaps[i].name + ": " + swaps[i].GetSwapState() + " / item " + swaps[i].GetCurrentItem().name + " at index " + swaps[i].GetCurrentIndex();

            // touch drag
            for (int i = 0; i < touchDragsTxt.Count; i++) touchDragsTxt[i].text = touchDrags[i].name + ": " + touchDrags[i].GetTouchDragState() + " / movement of " + touchDrags[i].GetTouchDragMovement();


            DoubleTap();
        }


        private void DoubleTap()
        {
            if (Input.GetMouseButtonDown(0))
            {
                float timeSinceLastTap = Time.time - lastTapTime;
                if (timeSinceLastTap < doubleTapTime) content.SetActive(!content.activeSelf);

                lastTapTime = Time.time;
            }
        }


        public void Logger(string _log) { Debug.Log(_log); }

        #endregion
    }
}