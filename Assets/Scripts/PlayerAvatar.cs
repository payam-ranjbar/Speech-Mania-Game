using System;
using System.Collections;
using UnityEngine;

public class PlayerAvatar: MonoBehaviour
{
        [SerializeField] private string stateName;
        [SerializeField] private GameObject mesh;
        public string StateName => stateName;

        private Coroutine _coroutine;
        private void OnEnable()
        {
                if(_coroutine != null) return;

                _coroutine = StartCoroutine(RotateContinuously());
        }

        private void OnDisable()
        {
                if(_coroutine is null) return;
                StopCoroutine(_coroutine);
                _coroutine = null;
        }

        public void SetState(PlayerState playerState)
        {
                stateName = playerState.stateName;
        }
        
        IEnumerator RotateContinuously()
        {
                while (true)
                {
                        transform.Rotate(0, 100 * Time.deltaTime, 0);
                        yield return null; 
                }
        }
}