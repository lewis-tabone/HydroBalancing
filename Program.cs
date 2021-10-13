readonly MyDefinitionId iceDef = MyDefinitionId.Parse("MyObjectBuilder_Ore/Ice");
IMyInventory cargo;
List<IMyGasGenerator> gens;
List<IMyGasTank> tanks;

public Program()
{
    gens = new List<IMyGasGenerator>();
    cargo = null;

    List<IMyTerminalBlock> gridBlocks = new List<IMyTerminalBlock>();
    tanks = new List<IMyGasTank>();

    GridTerminalSystem.GetBlocks(gridBlocks);
    foreach (IMyTerminalBlock block in gridBlocks)
    {
        if (block is IMyGasGenerator && block.IsSameConstructAs(Me))
        {
            (block as IMyGasGenerator).UseConveyorSystem = false;
            gens.Add(block as IMyGasGenerator);
        }
        if (block.CustomName.Contains("(Ice)") && block.IsSameConstructAs(Me))
        {
            cargo = block.GetInventory(0);
        }
        if (block is IMyGasTank && block.CustomName.Contains("Hydrogen"))
        {
            tanks.Add(block as IMyGasTank);
        }
    }

    Runtime.UpdateFrequency = UpdateFrequency.Update100;
}

public void Main(string argument, UpdateType updateSource)
{
    bool tankSpace = false; foreach (IMyGasTank tank in tanks)
    {
        if (tank.FilledRatio < 1)
        {
            tankSpace = true;
        }
    }
    if (tankSpace)
    {
        MyInventoryItem ice = (MyInventoryItem)cargo.FindItem(iceDef);

        foreach (var gen in gens)
        {
            if (gen.GetInventory(0).ItemCount == 0)
            {
                if (gen.CustomName.Contains("Enhanced"))
                    cargo.TransferItemTo(gen.GetInventory(0), ice, 2);
                else if (gen.CustomName.Contains("Proficient"))
                    cargo.TransferItemTo(gen.GetInventory(0), ice, 4);
                else if (gen.CustomName.Contains("Elite"))
                    cargo.TransferItemTo(gen.GetInventory(0), ice, 8);
                else if (gen.CustomName.Contains("Shield"))
                    cargo.TransferItemTo(gen.GetInventory(0), ice, 10);
                else
                    cargo.TransferItemTo(gen.GetInventory(0), ice, 1);
            }
        }
        Echo("Managing ice consumption as tanks fill...");
    }
    else
        Echo("Tanks are full. Standing by.");
}