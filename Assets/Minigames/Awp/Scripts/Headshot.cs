using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Awp {
    public class Headshot : MonoBehaviour {

        public AudioClip BoomHeadshotClip;

        AudioSource source;

        private void Awake() {
            source = GetComponent<AudioSource>();
        }

        private void OnMouseDown() {
            source.PlayOneShot(BoomHeadshotClip);
        }

    }
}