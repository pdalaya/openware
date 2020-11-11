using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Awp {
    public class Headshot : MonoBehaviour {

        public AudioClip BoomHeadshotClip;
        public AwpManager AwpManager;

        AudioSource source;

        private void Awake() {
            source = GetComponent<AudioSource>();
        }

        private void OnMouseDown() {
            source.PlayOneShot(BoomHeadshotClip);
            AwpManager.DidShootEnemy(transform.parent.gameObject);
        }
    }
}