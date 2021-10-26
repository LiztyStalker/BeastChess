using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectTransformExtend
{
    public static Vector2 GetRectTransformInWindow(Vector2 nowScreenPosition, RectTransform rectTr)
    {
        var position = nowScreenPosition;

        var posX = position.x;
        var posY = position.y;
        var rectWidth = rectTr.rect.width;
        var rectHeight = rectTr.rect.height;
        //var width = posX + rectTr.rect.width + rectTr.pivot.x;
        //var height = posY + rectTr.rect.height + rectTr.pivot.y;

        //Debug.Log($"{posX} {posY} {width} {height}");

        //벗어났는지 확인
        if(posX < rectWidth * rectTr.pivot.x || posX + (rectWidth * (rectTr.pivot.x - 1) * -1) > Screen.width)
        {
            //벗어났으면 보정 (상하좌우)
            if (posX < rectWidth * rectTr.pivot.x)
            {
                posX += rectWidth - posX;
            }
            else if (posX + (rectWidth * (rectTr.pivot.x - 1) * -1) > Screen.width)
            {
                posX += (Screen.width - (posX + rectWidth));
            }
            position.x = posX;
        }
        if (posY < rectHeight * rectTr.pivot.y || posY + (rectHeight * (rectTr.pivot.y - 1) * -1) > Screen.height)
        {
            //벗어났으면 보정 (상하좌우)
            if (posY < rectHeight * rectTr.pivot.y)
            {
                posY += rectHeight - posY;
            }
            else if (posY + (rectHeight * (rectTr.pivot.y - 1) * -1) > Screen.height)
            {
                posY += (Screen.height - (posY + rectHeight));
            }
            position.y = posY;
        }

        //보정된 값 출력
        //안 벗어났으면
        //그대로 출력
        return position;
    }
}
   
