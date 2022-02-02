using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace TestUtils {
    public class InputPress : IDisposable {
        private readonly InputTestFixture input;
        private readonly ButtonControl[] controls;
        public InputPress(InputTestFixture input, params ButtonControl[] controls) {
            this.input = input;
            this.controls = controls;
            foreach (var control in controls) {
                input.Press(control, queueEventOnly: true);
            }
        }
        public void Dispose() {
            foreach (var control in controls) {
                input.Release(control, queueEventOnly: true);
            }
        }
    }
}