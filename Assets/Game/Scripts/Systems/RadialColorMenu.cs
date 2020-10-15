using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Shapes;
using UnityEngine;

namespace Systems
{
    public class RadialColorMenu : MonoBehaviour
    {
        [SerializeField] private List<ColorMenuItem> colorItems = default;

        [SerializeField] private Disc background = default;

        [SerializeField] private Disc picked = default;

        [SerializeField] private Disc pickedBackground = default;

        [SerializeField] private Disc indicator = default;

        private bool initialized;

        [Header("Shape Settings")] [SerializeField]
        private float _colorSpacing = 1f;

        [SerializeField] private float _darkenAmount = .5f;

        [Space] [SerializeField] private float _indicatorRadius = default;

        [SerializeField] private float _indicatorThickness = default;

        [Space] [SerializeField] private float _backgroundRadius = default;

        [SerializeField] private float _backgroundThickness = default;

        [SerializeField] private float _colorRadius = default;

        [SerializeField] private float _colorThickness = default;

        [Space] [SerializeField] private float _pickedBackgroundRadius = default;

        [SerializeField] private float _pickedBackgroundThickness = default;

        [SerializeField] private float _pickedRadius = default;

        [SerializeField] private float _pickedThickness = default;

        private Coroutine runningCoroutine = null;

        private States state = States.Initializing;

        private int selectedItem;

        public enum States
        {
            Initializing,
            Selecting,
            Selected,
            Idle,
            Hiding,
            Hidden,
            Revealing
        }

        public void DrawMenu()
        {
            switch (state)
            {
                case States.Initializing:
                    Init();
                    break;
                case States.Revealing:
                    break;
                case States.Selecting:
                    DrawWheel();
                    DrawSelection();
                    break;
                case States.Selected:
                    DrawWheel();
                    Selected();
                    break;
                case States.Idle:
                    break;
            }
        }

        private void Selected()
        {
            if (runningCoroutine == null) runningCoroutine = StartCoroutine(SelectColorCoroutine());
        }

        public void Select()
        {
            selectedItem = colorItems.FindIndex(x => x.color == picked.Color);
            state = States.Selected;
        }

        private IEnumerator SelectColorCoroutine()
        {
            throw new NotImplementedException();
            // List<ColorMenuItem> items = colorItems.Where(x => x.weight > 0)
            //     .ToList();
            //
            // int val = items.FindIndex(x => x.visualPiece == colorItems[selectedItem].visualPiece);
            // for (int i = 0; i < items.Count; i++)
            // {
            //     float x = i;
            //     float y = items.Count;
            //     float z = selectedItem;
            //     float weightIntensity = 3f;
            //
            //     StartCoroutine(ChangeWeightOverTimeCoroutine(items[i], items[i].weight,
            //         Mathf.Cos(((Mathf.PI * 2 * x) / y) - (z * Mathf.PI) - (z * Mathf.PI)) + 1.5f,
            //         1f));
            // }
            //
            //
            // yield return null;
        }

        private void DrawSelection()
        {
            Vector2 relativeMouseDirection =
                UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            Debug.DrawRay(transform.position, relativeMouseDirection, Color.white);

            float selectionDirection =
                transform.InverseTransformDirection(relativeMouseDirection).AsVector2().Angle() * Mathf.Deg2Rad;
            indicator.AngRadiansStart = indicator.AngRadiansEnd = selectionDirection;

            foreach (ColorMenuItem colorMenuItem in colorItems)
            {
                if (colorMenuItem.visualPiece.AngRadiansStart < indicator.AngRadiansStart &&
                    colorMenuItem.visualPiece.AngRadiansEnd > indicator.AngRadiansEnd)
                {
                    colorMenuItem.visualPiece.ColorInner = new Color(
                        colorMenuItem.color.r * _darkenAmount,
                        colorMenuItem.color.g * _darkenAmount,
                        colorMenuItem.color.b * _darkenAmount,
                        colorMenuItem.color.a);
                    picked.Color = colorMenuItem.color;
                    picked.ColorOuter = colorMenuItem.visualPiece.ColorInner;
                }
                else
                    colorMenuItem.visualPiece.ColorInner = colorMenuItem.color;
            }
        }

        private void Init()
        {
            //initialize based on initial values
            _indicatorRadius = indicator.Radius;
            _indicatorThickness = indicator.Thickness;

            _backgroundRadius = background.Radius;
            _backgroundThickness = background.Thickness;

            _colorRadius = colorItems[0].visualPiece.Radius;
            _colorThickness = colorItems[0].visualPiece.Thickness;

            _pickedRadius = picked.Radius;
            _pickedThickness = picked.Thickness;
            _pickedBackgroundRadius = pickedBackground.Radius;
            _pickedBackgroundThickness = pickedBackground.Thickness;

            gameObject.SetActive(true);

            background.Radius = background.Thickness = 0f;
            picked.Radius = picked.Thickness = 0f;
            pickedBackground.Radius = pickedBackground.Thickness = 0f;
            indicator.Radius = indicator.Thickness = 0f;

            foreach (ColorMenuItem items in colorItems)
            {
                items.visualPiece.Thickness = items.visualPiece.Radius = 0f;
            }

            state = States.Hidden;
        }

        private void DrawWheel()
        {
            List<ColorMenuItem> items = colorItems.Where(x => x.weight > 0f).ToList();


            float totalWeight = items.Where(x => x.weight > 0f).Sum(x => x.weight);
            float singleWeightDegrees = 360f / totalWeight;

            float currentDegrees = 0;
            foreach (ColorMenuItem colorMenuItem in colorItems)
            {
                colorMenuItem.visualPiece.Radius = _colorRadius;
                colorMenuItem.visualPiece.Thickness = _colorThickness;

                float space = colorMenuItem.weight > 0 ? _colorSpacing : 0;
                colorMenuItem.visualPiece.AngRadiansStart = (currentDegrees * Mathf.Deg2Rad) + space * Mathf.Deg2Rad;
                colorMenuItem.visualPiece.AngRadiansEnd =
                    ((currentDegrees + (singleWeightDegrees * colorMenuItem.weight)) * Mathf.Deg2Rad) -
                    space * Mathf.Deg2Rad;
                colorMenuItem.visualPiece.Color = colorMenuItem.color;

                currentDegrees += (singleWeightDegrees * colorMenuItem.weight);
            }
        }

        public void Reveal(float duration)
        {
            if (runningCoroutine == null) runningCoroutine = StartCoroutine(RevealCoroutine(duration));
        }

        public IEnumerator RevealCoroutine(float duration)
        {
            Debug.Log("revealing");
            state = States.Revealing;

            // Make background appear
            for (float timePassed = 0; timePassed < duration; timePassed += Time.deltaTime)
            {
                float progress = timePassed / duration;
                background.Thickness = Mathf.Lerp(0, _backgroundThickness, progress);
                background.Radius = Mathf.Lerp(0, _backgroundRadius, progress);

                indicator.Thickness = Mathf.Lerp(0, _indicatorThickness, progress);
                indicator.Radius = Mathf.Lerp(0, _indicatorRadius, progress);

                picked.Thickness = Mathf.Lerp(0, _pickedThickness, progress);
                picked.Radius = Mathf.Lerp(0, _pickedRadius, progress);

                pickedBackground.Thickness = Mathf.Lerp(0, _pickedBackgroundThickness, progress);
                pickedBackground.Radius = Mathf.Lerp(0, _pickedBackgroundRadius, progress);

                foreach (ColorMenuItem colorMenuItem in colorItems)
                {
                    colorMenuItem.visualPiece.Thickness = Mathf.Lerp(0, _colorThickness, progress);
                    colorMenuItem.visualPiece.Radius = Mathf.Lerp(0, _colorRadius, progress);
                }

                yield return null;
            }

            background.Thickness = _backgroundThickness;
            background.Radius = _backgroundRadius;
            indicator.Thickness = _indicatorThickness;
            indicator.Radius = _indicatorRadius;
            foreach (ColorMenuItem colorMenuItem in colorItems)
            {
                colorMenuItem.visualPiece.Thickness = _colorThickness;
                colorMenuItem.visualPiece.Radius = _colorRadius;
            }

            state = States.Selecting;
            runningCoroutine = null;
        }


        public IEnumerator ChangeWeightOverTimeCoroutine(ColorMenuItem item, float start, float target, float duration)
        {
            for (float timePassed = 0; timePassed < duration; timePassed += Time.deltaTime)
            {
                item.weight = Mathf.Lerp(start, target, timePassed / duration);
                yield return null;
            }

            item.weight = target;
        }

        [Serializable]
        public class ColorMenuItem
        {
            public Color color;

            public float weight;

            public Disc visualPiece;
        }

        public void ToggleSecondaryColors()
        {
            ToggleSecondaryColors(!(colorItems[1].weight > 0f));
        }

        public void ToggleSecondaryColors(bool toggle)
        {
            StartCoroutine(ChangeWeightOverTimeCoroutine(colorItems[1], toggle ? 0 : 1, toggle ? 1 : 0, 1f));
            StartCoroutine(ChangeWeightOverTimeCoroutine(colorItems[3], toggle ? 0 : 1, toggle ? 1 : 0, 1f));
            StartCoroutine(ChangeWeightOverTimeCoroutine(colorItems[5], toggle ? 0 : 1, toggle ? 1 : 0, 1f));
        }
    }
}