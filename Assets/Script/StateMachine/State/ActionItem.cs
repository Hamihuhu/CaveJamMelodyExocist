using UnityEngine;

    public class ActionItem
    {

        public enum ActionInput {NormalAttack,HoldAttack,Jump,Dash };
        public ActionInput Action;
        public float Timestamp;
        public float timeToNextBuffer;
        public static float TimeBeforeActionsExpire = 2f;

        public ActionItem(ActionInput ia, float stamp,float timeToNextBuffer)
        {
            Action = ia;
            Timestamp = stamp;
             this.timeToNextBuffer = timeToNextBuffer;
    }

        public bool CheckIfValid()
        {
            bool returnValue = false;
            if (Timestamp + TimeBeforeActionsExpire >= Time.time)
            {
                returnValue = true;
            }
            return returnValue;
            }
    }
