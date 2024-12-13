using UnityEngine;

public struct State
{
    //  The proportion of liquidity not spent in a turn.
    static readonly float SAVING_RATE = 0.25f;
    //  How much leftover investment gets put into increasing development
    static readonly float DEVELOPMENT_RATE = 0.25f;
    //  How much new development is created per liquidity invested.
    static readonly float DEVELOPMENT_EFFICIENCY = 1;
    //  How much leftover investment gets put into increasing technology
    static readonly float RESEARCH_RATE = 0.25f;
    //  How much the tech level increases per liquidity invested.
    static readonly float RESEACH_EFFICIENCY = 1;
    //  How much leftover investment gets put into increasing infrastructure
    static readonly float INFRASTRUCTURE_RATE = 0.25f;
    //  How much the infrastructure level increases per liquidity invested.
    static readonly float INFRASTRUTURE_EFFICIENCY = 1;
    //  How much unused development decays.
    static readonly float DEVELOPMENT_DECAY_RATE = 0.5f;
    //  How much investment infrastructure needs to be maintained.
    static readonly float INFRASTRUCTURE_UPKEEP_RATE = 0.5f;
    //  How much unmaintained infrastructure decays.
    static readonly float INFRASTRUCTURE_DECAY_RATE = 0.5f;
    //  How much free money is in the economy that can be used.
    public float liquidity;
    //  The tech level, controls how efficient turning investment in production into new wealth is.
    public float technology;
    //  How much investment in production (new wealth creation) can be sustained.
    public float development;
    //  The cap on the development level.
    public float infrastructure;
    //  Advance by a turn.
    //  First, some investment money is taken from the liquidity.
    //  Then, the investment is put into production, with the maximum put into production being the development.
    //  The remaining investment is split into increasing development and technology (with some being wasted).
    //  The amount of new wealth created is the investment in production multiplied by the technology level.
    public readonly State Next()
    {

        (float saved, float toSpend) = (SAVING_RATE * liquidity, (1 - SAVING_RATE) * liquidity);
        if (toSpend > development)
        {
            toSpend -= development;
            float infraInvestment = INFRASTRUCTURE_RATE * toSpend;
            float infraUpkeep = INFRASTRUCTURE_UPKEEP_RATE * infrastructure;
            return new()
            {
                liquidity = saved + development * technology,
                technology = technology + RESEACH_EFFICIENCY * RESEARCH_RATE * toSpend,
                development = Mathf.Max(infrastructure, development + DEVELOPMENT_EFFICIENCY * DEVELOPMENT_RATE * toSpend),
                infrastructure = infrastructure 
                    + (infraUpkeep > infraInvestment ? INFRASTRUCTURE_DECAY_RATE : INFRASTRUTURE_EFFICIENCY) 
                    * (infraInvestment - infraUpkeep),
            };
        }
        else
        {
            return new()
            {
                liquidity = saved + toSpend * technology,
                technology = technology,
                development = Mathf.Max(infrastructure, development - (development - toSpend) * DEVELOPMENT_DECAY_RATE),
                infrastructure = infrastructure * INFRASTRUCTURE_DECAY_RATE,
            };
        }
    }
}
