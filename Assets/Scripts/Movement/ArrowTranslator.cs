using UnityEngine;

public class ArrowTranslator
{
    public enum ArrowDirection
    {
        None = 0,
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4,
        TopLeft = 5,
        BottomLeft = 6,
        TopRight = 7,
        BottomRight = 8,
        UpFinished = 9,
        DownFinished = 10,
        LeftFinished = 11,
        RightFinished = 12
    }

    //  This function checks the tiles where the character is, the next one and the previous one and decides which image should be printed
    public ArrowDirection TranslateDirection(OverlayTile previousTile, OverlayTile currentTile, OverlayTile futureTile)
    {
        bool isFinal = futureTile == null;

        Vector2Int pastDirection = previousTile != null ? (Vector2Int)(currentTile.GetGridLocation() - previousTile.GetGridLocation()) : new Vector2Int(0, 0);
        Vector2Int futureDirection = futureTile != null ? (Vector2Int)(futureTile.GetGridLocation() - currentTile.GetGridLocation()) : new Vector2Int(0, 0);
        Vector2Int direction = pastDirection != futureDirection ? pastDirection + futureDirection : futureDirection;

        if (direction == new Vector2(0, 1) && !isFinal)
        {
            return ArrowDirection.Up;
        }

        if (direction == new Vector2(0, -1) && !isFinal)
        {
            return ArrowDirection.Down;
        }

        if (direction == new Vector2(1, 0) && !isFinal)
        {
            return ArrowDirection.Right;
        }

        if (direction == new Vector2(-1, 0) && !isFinal)
        {
            return ArrowDirection.Left;
        }

        if (direction == new Vector2(1, 1))
        {
            if (pastDirection.y < futureDirection.y)
                return ArrowDirection.BottomLeft;
            else
                return ArrowDirection.TopRight;
        }

        if (direction == new Vector2(-1, 1))
        {
            if (pastDirection.y < futureDirection.y)
                return ArrowDirection.BottomRight;
            else
                return ArrowDirection.TopLeft;
        }

        if (direction == new Vector2(1, -1))
        {
            if (pastDirection.y > futureDirection.y)
                return ArrowDirection.TopLeft;
            else
                return ArrowDirection.BottomRight;
        }

        if (direction == new Vector2(-1, -1))
        {
            if (pastDirection.y > futureDirection.y)
                return ArrowDirection.TopRight;
            else
                return ArrowDirection.BottomLeft;
        }

        if (direction == new Vector2(0, 1) && isFinal)
        {
            return ArrowDirection.UpFinished;
        }

        if (direction == new Vector2(0, -1) && isFinal)
        {
            return ArrowDirection.DownFinished;
        }

        if (direction == new Vector2(-1, 0) && isFinal)
        {
            return ArrowDirection.LeftFinished;
        }

        if (direction == new Vector2(1, 0) && isFinal)
        {
            return ArrowDirection.RightFinished;
        }

        return ArrowDirection.None;
    }


}
