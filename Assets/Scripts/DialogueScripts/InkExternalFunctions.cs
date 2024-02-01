using System.Collections;
using UnityEngine;
using Ink.Runtime;

namespace Assets.Scripts.DialogueScripts
{
    public class InkExternalFunctions
    {
        public void Bind(Story story, GameObject obj)
        {
            /*story.BindExternalFunction("openForest", (string objname) =>
            {
                obj.SetActive(true);
            });*/
        }

        public void Unbind(Story story)
        {

        }
    }
}