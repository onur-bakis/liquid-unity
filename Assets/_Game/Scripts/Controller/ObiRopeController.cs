using System;
using System.Collections;
using System.Collections.Generic;
using Obi;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

namespace Scripts.Controller
{
    public class ObiRopeController : MonoBehaviour
    {
        [SerializeField] private TextMeshPro ropeCutCountText;
        public ObiRope rope;

        public int ropeBreakValue;

        private void Start()
        {
            SetText();
        }

        private void Update()
        {
            PositionTextObject();
        }

        public void SetBreakValue(int ropeBreakValue)
        {
            this.ropeBreakValue = ropeBreakValue;
            SetText();
        }

        private void SetText()
        {
            ropeCutCountText.text = ropeBreakValue.ToString();
        }

        private void PositionTextObject()
        {
            var elm = rope.GetElementAt(0.5f, out float elementMu);
            var position = Vector3.Lerp(rope.solver.positions[elm.particle1], rope.solver.positions[elm.particle2], elementMu);

            ropeCutCountText.transform.position = position;
        }

        public void BreakRope(Vector3 position)
        {
            ropeCutCountText.gameObject.SetActive(false);
            FindClosetElementAndBreak(position);
        }

        private void FindClosetElementAndBreak(Vector3 position)
        {
            float minDistance = float.MaxValue;
            int closetRopeElementIndex = 0;
            
            for (int i = 0; i < rope.elements.Count; ++i)
            {
                // project the both ends of the element to screen space.
                Vector3 elementStart = rope.solver.positions[rope.elements[i].particle1];
                Vector3 elementEnd = rope.solver.positions[rope.elements[i].particle2];

                Vector3 middle = (elementStart + elementEnd) / 2f;

                float distance = Vector3.Distance(position,middle);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closetRopeElementIndex = i;
                }
            }

            rope.Tear(rope.elements[closetRopeElementIndex]);
            rope.RebuildConstraintsFromElements();
            
            Invoke(nameof(RopeDisable),1f);
        }

        public void RopeDisable()
        {
            rope.gameObject.SetActive(false);
        }
    }
}

