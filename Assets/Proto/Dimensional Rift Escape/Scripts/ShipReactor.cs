﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Kpable.Proto.DimensionalRiftEscape
{
    public class ShipReactor : MonoBehaviour
    {

        // 18 19 28
        public GameObject[] runes;

        int[] RunePassword = { 18, 19, 28 };

        int[] passwordEntry = new int[3];

        int passindex = 0;

        AudioSource source;
        // Use this for initialization
        void Start()
        {
            source = GetComponent<AudioSource>();
            for (int i = 0; i < runes.Length; i++)
            {
                runes[i].GetComponent<Rune>().OnRuneHit += RuneHit;
            }

        }

        // Update is called once per frame
        void Update()
        {

        }

        void RuneHit(int runeID)
        {
            Debug.Log(runeID + " was hit");

            source.PlayOneShot(GameManager.Instance.clips[6]);

            passwordEntry[passindex] = runeID;
            passindex++;
            if (passindex == passwordEntry.Length)
                if (ValidatePassword())
                    CorrectPassword();
                else
                    ResetRunes();
        }

        bool ValidatePassword()
        {
            for (int i = 0; i < RunePassword.Length; i++)
            {
                if (passwordEntry[i] == RunePassword[i])
                    continue;
                else
                    return false;
            }

            return true;
        }

        void CorrectPassword()
        {
            Debug.Log("The password is correct");
            source.PlayOneShot(GameManager.Instance.clips[8]);


        }

        void ResetRunes()
        {
            Debug.Log("Wrong password reset runes");
            source.PlayOneShot(GameManager.Instance.clips[7]);

            for (int i = 0; i < runes.Length; i++)
            {
                runes[i].GetComponent<Rune>().Unpress();
            }
            passindex = 0;
        }
    }
}