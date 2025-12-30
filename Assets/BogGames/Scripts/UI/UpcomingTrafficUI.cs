using UnityEngine;
using BogGames.Gameplay;
using UnityEngine.UI;
using BogGames.Variables;

namespace BogGames.UI
{
    public class UpcomingTrafficUI : MonoBehaviour
    {
        public Channel channel = Channel.Bridge;
        public Transform trafficContent;
        public CanvasGroup parentGroup;
        public Vector2 iconScale = new Vector2(300, 300);
        public MovingCarVariable spawnedCar;
        public MovingCarVariable disabledCar;

        [Header("Variables")]
        public BogSideVariable playerSide;
        public BogBooleanVariable playerCrossing;

        protected void Update()
        {
            if(parentGroup == null)
                return;

            if(playerCrossing.Value == true)
                parentGroup.alpha = 0;
            else
                parentGroup.alpha = 1;

                var cg = trafficContent.GetComponent<CanvasGroup>();
                if(cg == null)
                    return;

            if(AreOpposite(channel, playerSide.Value))
                cg.alpha = 1;
            else
                cg.alpha = 0;
        }

        bool AreOpposite(Channel channel, Side side)
        {
            return (channel == Channel.BelowLeft && side == Side.Right) ||
                (channel == Channel.BelowRight && side == Side.Left);
        }

        public void AddIconToContent()
        {
            if(spawnedCar && spawnedCar.Value != null)
            {
                var mv = spawnedCar.Value as WavingCar;
                if(mv == null || trafficContent == null)
                    return;

                if(mv.channel == this.channel)
                {
                    if(mv.CarSprite != null)
                    {
                        GameObject imageObj = new GameObject("CarIcon");
                        Image image = imageObj.AddComponent<Image>();
                        image.sprite = mv.CarSprite;
                        image.preserveAspect = true;
                        imageObj.transform.SetParent(trafficContent, false);
                        imageObj.GetComponent<RectTransform>().sizeDelta = iconScale;
                    }
                }
            }            
        }

        public void RemoveIconFromDisabled()
        {
            if(disabledCar && disabledCar.Value != null)
            {
                var mv = disabledCar.Value as WavingCar;

                if(mv == null)
                    return;
                
                if(mv.channel == this.channel)
                {
                    if(trafficContent)
                    {
                        if(trafficContent.transform.childCount <= 0)
                            return;

                        var child = trafficContent.transform.GetChild(0);
                        if(child != null)
                        {
                            Destroy(child.gameObject);
                        }
                    }
                }
            }
        }
    }
}
