using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Source.Core
{
    public class UserInterface : MonoBehaviour
    {
        public enum TextPosition : byte
        {
            TopLeft,
            TopCenter,
            TopRight,
            MiddleLeft,
            MiddleCenter,
            MiddleRight,
            BottomLeft,
            BottomCenter,
            BottomRight
        }

        public static UserInterface Singleton { get; private set; }

        private TextMeshProUGUI[] _textComponents;

        private void Awake()
        {
            if (Singleton != null)
            {
                Destroy(this);
                return;
            }
            
            Singleton = this;

            _textComponents = GetComponentsInChildren<TextMeshProUGUI>();
        }

        public void SetText(string text, TextPosition textPosition)
        {
            _textComponents[(int) textPosition].text = text;
        }

        public void RemoveText(TextPosition textPosition)
        {
            _textComponents[(int)textPosition].text = "";
        }

        public async void HandleTextDisplay(string text, TextPosition textPosition)
        {
            SetText(text, textPosition);
            await Task.Delay(2100);
            RemoveText(textPosition);
        }


    }
}
