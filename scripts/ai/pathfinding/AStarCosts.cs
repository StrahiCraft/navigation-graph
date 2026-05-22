using System;

public struct AStarCosts : IComparable
{
	/// <summary>
	/// Distance we traversed to get here
	/// </summary>
	private float _hCost;
	/// <summary>
	/// Distance to the goal
	/// </summary>
	private float _gCost;

	public readonly float HCost { get => _hCost; }
	public readonly float GCost { get => _gCost; }
	public readonly float FCost { get => _hCost + _gCost; }
	
	public AStarCosts(float hCost, float gCost)
	{
		_hCost = hCost;
		_gCost = gCost;
	}

    public int CompareTo(object obj)
    {
        AStarCosts otherCosts = (AStarCosts) obj;

		if(otherCosts.FCost == FCost)
		{
			return 0;
		}

		if(otherCosts.FCost > FCost)
		{
			return -1;
		}

		return 1;
    }

}
