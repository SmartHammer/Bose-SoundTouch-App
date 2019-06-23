using BoseSoundTouchApp.Models;
using System.Collections.Generic;

namespace BoseSoundTouchApp.Services
{
    public class POST_key : POST
    {
        public class KeyType
        {
            public KeyType(Keys key, States state)
            {
                Value = key;
                State = state;
            }

            public enum Keys : int
            {
                  PLAY
                , PAUSE
                , PLAY_PAUSE
                , STOP
                , PREV_TRACK
                , NEXT_TRACK
                , POWER
                , MUTE
                , AUX_INPUT
                , SHUFFLE_ON
                , SHUFFLE_OFF
                , REPEAT_ONE
                , REPEAT_ALL
                , REPEAT_OFF
                , ADD_FAVORITE
                , REMOVE_FAVORITE
                , THUMBS_UP
                , THUMBS_DOWN
                , BOOKMARK
                , PRESET_1
                , PRESET_2
                , PRESET_3
                , PRESET_4
                , PRESET_5
                , PRESET_6
            }

            public enum States
            {
                  PRESS
                , RELEASE
            }

            public Keys Value
            {
                get;
                private set;
            }

            public States State
            {
                get;
                private set;
            }
        }

        public POST_key(IPhysicalData physicalData)
            : base(physicalData)
        {
        }

        public KeyType Key
        {
            set
            {
                Node node = new Node("key", value.Value.ToString())
                {
                    Attributes = new Dictionary<string, string> {
                      { "state", value.State.ToString().ToLower() }
                    , { "sender", "Gabbo" }
                }
                };

                Post(node);
            }
        }
    }
}
