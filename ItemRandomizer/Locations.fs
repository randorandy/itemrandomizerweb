namespace ItemRandomizer
module Locations =
    open Types

// open: the folder in console
// type: dotnet run
// navigate to: localhost:8888

// This is the Master otherRotation logic
// version 0.3 of total's updated Aug 20 2022 by ironrusty

    // Functions to check if we have a specific item
    let haveItem items (itemType:ItemType) =
        List.exists (fun (item:Item) -> item.Type = itemType) items
    
    let itemCount items itemType =
        List.length (List.filter (fun (item:Item) -> item.Type = itemType) items)

    let energyReserveCount items =
        itemCount items ETank +
        itemCount items Reserve

    let heatProof items =
        haveItem items Varia

    // Combined checks to see if we can perform an action needed to access locations
    

    let canFly items = (haveItem items Morph && haveItem items Bomb) || haveItem items SpaceJump
    let canUseBombs items = haveItem items Morph && haveItem items Bomb

    let canOpenRedDoors items = haveItem items Missile || haveItem items Super
    let canOpenGreenDoors items = haveItem items Super
    let canOpenYellowDoors items = haveItem items Morph && haveItem items PowerBomb
    let canUsePowerBombs = canOpenYellowDoors

    let canDestroyBombWalls items =
        (haveItem items Morph &&
            (haveItem items Bomb ||
             haveItem items PowerBomb)) ||
        haveItem items ScrewAttack
    
    let canCrystalFlash items = 
        itemCount items Missile >= 2 &&
        itemCount items Super >= 2 &&
        itemCount items PowerBomb >= 3

    let canCrystalFlashTwice items = 
        itemCount items Missile >= 4 &&
        itemCount items Super >= 4 &&
        itemCount items PowerBomb >= 5

    let canHellRunTwo items = 
        energyReserveCount items >= 3 ||
        (energyReserveCount items >= 2 && canCrystalFlash items) ||
        heatProof items

    let canHellRunThree items = 
        energyReserveCount items >= 3 ||
        (energyReserveCount items >= 2 && canCrystalFlash items) ||
        heatProof items

    let canHellRunFour items = 
        energyReserveCount items >= 4 ||
        (energyReserveCount items >= 3 && canCrystalFlash items) ||
        heatProof items

    let canHellRunFive items = 
        energyReserveCount items >= 5 ||
        (energyReserveCount items >= 4 && canCrystalFlash items) ||
        heatProof items

    let canHellRunSix items = 
        energyReserveCount items >= 6 ||
        (energyReserveCount items >= 4 && canCrystalFlash items) ||
        (energyReserveCount items >= 3 && canCrystalFlashTwice items) ||
        heatProof items

    let canHellRunSeven items = 
        energyReserveCount items >= 7 ||
        (energyReserveCount items >= 5 && canCrystalFlash items) ||
        (energyReserveCount items >= 4 && canCrystalFlashTwice items) ||
        heatProof items

    let canHellRunEight items = 
        energyReserveCount items >= 8 ||
        (energyReserveCount items >= 5 && canCrystalFlash items) ||
        (energyReserveCount items >= 4 && canCrystalFlashTwice items) ||
        heatProof items

    let canHellRunTwelve items = 
        energyReserveCount items >= 12 ||
        (energyReserveCount items >= 7 && canCrystalFlash items) ||
        (energyReserveCount items >= 5 && canCrystalFlashTwice items) ||
        heatProof items
    
    let canPassBombPassages items =
           canUseBombs items || 
           canUsePowerBombs items

    let canEnterAndLeaveGauntlet items =
        canPassBombPassages items &&
        haveItem items Varia &&
        haveItem items Gravity &&
        ((haveItem items ScrewAttack && energyReserveCount items >= 4) ||
            (canUsePowerBombs items && energyReserveCount items >= 8))
    
    let canAccessRedBrinstar items =
        haveItem items Super &&
        haveItem items Morph &&
            (canDestroyBombWalls items || 
             canUsePowerBombs items)
    // note, morph is not required to enter red brin
    // supers (traditional) or power bombs (meme route) required
    
    let canAccessKraid items = 
        canAccessRedBrinstar items &&
        (haveItem items HiJump ||
            canFly items) &&
        (canPassBombPassages items ||
            (haveItem items Morph &&
             haveItem items ScrewAttack &&
             haveItem items SpringBall))
    
    let canAccessWs items = 
        canUsePowerBombs items && 
        haveItem items Super &&
        (haveItem items SpringBall ||
            haveItem items HiJump ||
            haveItem items SpeedBooster ||
            haveItem items Gravity ||
            canUseBombs items)
        // moat options or forgotten highway with UWIBJ

    let canDefeatPhantoon items =
        canAccessWs items

    let canPassAttic items =
        // also considers upper ocean HJB or gravity
        canDefeatPhantoon items 

    let canAccessHeatedNorfair items =
        // that is bubble mt thru cathedral
        canAccessRedBrinstar items &&
        ((canHellRunSix items && haveItem items HiJump) ||
            (haveItem items SpeedBooster && energyReserveCount items >= 2))
    
    let canAccessCrocomire items =
        (canAccessHeatedNorfair items &&
            haveItem items Wave &&
            canPassBombPassages items &&
            energyReserveCount items >= 3 &&
            (haveItem items Missile ||
                haveItem items Charge)) ||
            (canAccessRedBrinstar items &&
                haveItem items Varia &&
                haveItem items SpeedBooster &&
                energyReserveCount items >= 1)
        // Can enter from wave gate or speed croc speedway
    
    let canAccessLowerNorfair items = 
        canAccessHeatedNorfair items &&
        canUsePowerBombs items &&
        haveItem items Varia
        // leave Varia in for now...
    
    let canPassWorstRoom items =
        // which is now ampitheater
        canAccessLowerNorfair items &&
        ((haveItem items HiJump && haveItem items SpeedBooster) ||
            haveItem items SpaceJump ||
            (haveItem items HiJump &&
                haveItem items Gravity &&
                energyReserveCount items >= 5))
        //with gravity + HJB, grav jump in acid!!

    let canAccessOuterMaridia items = 
        canAccessRedBrinstar items &&
        canUsePowerBombs items
        //break the tube

    let canAccessInnerMaridia items =
        //climb Mt. Everest
        canAccessOuterMaridia items &&
        (haveItem items HiJump ||
            haveItem items Grapple ||
            haveItem items Gravity ||
            canUseBombs items)
        // UWIBJ included
    
    let canDoSuitlessMaridia items =
        // NA for now
         (haveItem items HiJump && (haveItem items Ice || haveItem items SpringBall) && haveItem items Grapple)

    let canDefeatBotwoon items =
        // Let's consider defeating botwoon OR sand bypass
        canAccessInnerMaridia items &&
            (haveItem items Charge ||
                haveItem items Gravity ||
                (haveItem items HiJump && haveItem items SpringBall) ||
                canUseBombs items)
        // can do HJB + SB in Master, and UWIBJ

    let canDefeatDraygon items =
        canDefeatBotwoon items;
            //escape is free

    // Item Locations
    let AllLocations = 
        [
            {
                Area = Crateria;
                Name = "Power Bomb (Crateria surface)";
                Class = Minor;
                Address = 0x781CC;
                Visibility = Visible;
                Available = fun items ->
                    canUsePowerBombs items &&
                    (canFly items ||
                        haveItem items SpeedBooster);
            };
            {
                Area = Crateria;
                Name = "Missile (outside Wrecked Ship bottom)";
                Class = Minor;
                Address = 0x781E8;
                Visibility = Visible;
                Available = fun items -> canAccessWs items;
            };
            {
                Area = Crateria;
                Name = "Missile (outside Wrecked Ship top)";
                Class = Minor;
                Address = 0x781EE;
                Visibility = Hidden;
                Available = fun items -> canPassAttic items;
            };
            {
                Area = Crateria;
                Name = "Missile (outside Wrecked Ship middle)";
                Class = Minor;
                Address = 0x781F4;
                Visibility = Visible;
                Available = fun items -> canPassAttic items;
            };
            {
                Area = Crateria;
                Name = "Missile (Crateria moat)";
                Class = Minor
                Address = 0x78248;
                Visibility = Visible;
                Available = fun items -> canUsePowerBombs items;
            };
            {
                Area = Crateria;
                Name = "Energy Tank, Gauntlet";
                Class = Major;
                Address = 0x78264;
                Visibility = Visible;
                Available = fun items -> canEnterAndLeaveGauntlet items;
            };
            {
                Area = Crateria;
                Name = "Missile (Crateria bottom)";
                Class = Minor;
                Address = 0x783EE;
                Visibility = Visible;
                Available = fun items -> canPassBombPassages items;
            };
            {
                Area = Crateria;
                Name = "Bomb";
                Address = 0x78404;
                Class = Major;
                Visibility = Chozo;
                Available = fun items -> haveItem items Morph &&
                                            canOpenRedDoors items;
            };
            {
                Area = Crateria;
                Name = "Energy Tank, Terminator";
                Class = Major;
                Address = 0x78432;
                Visibility = Visible;
                Available = fun items -> canDestroyBombWalls items;
            };
            {
                Area = Crateria;
                Name = "Missile (Crateria gauntlet right)";
                Class = Minor;
                Address = 0x78464;
                Visibility = Visible;
                Available = fun items -> canEnterAndLeaveGauntlet items;
            };
            {
                Area = Crateria;
                Name = "Missile (Crateria gauntlet left)";
                Class = Minor;
                Address = 0x7846A;
                Visibility = Visible;
                Available = fun items -> canEnterAndLeaveGauntlet items;
            };
            {
                Area = Crateria;
                Name = "Super Missile (Crateria)";
                Class = Minor;
                Address = 0x78478;                
                Visibility = Visible;
                Available = fun items -> canUsePowerBombs items && 
                                         haveItem items SpeedBooster; 
            };
            {
                Area = Crateria;
                Name = "Missile (Crateria middle)";
                Class = Minor;
                Address = 0x78486;
                Visibility = Visible;
                Available = fun items -> canDestroyBombWalls items;
            };
            {
                Area = Brinstar;
                Name = "Power Bomb (green Brinstar bottom)";
                Class = Minor;
                Address = 0x784AC;
                Visibility = Chozo;
                Available = fun items -> canUsePowerBombs items &&
                                         (canFly items ||
                                            haveItem items HiJump ||
                                            haveItem items SpeedBooster); 
            };
            {
                Area = Brinstar;
                Name = "Super Missile (pink Brinstar)";
                Class = Minor;
                Address = 0x784E4;
                Visibility = Chozo;
                Available = fun items -> canDestroyBombWalls items &&
                                         haveItem items Super;
            };
            {
                Area = Brinstar;
                Name = "Missile (green Brinstar below super missile)";
                Class = Minor;
                Address = 0x78518;
                Visibility = Visible;
                Available = fun items -> canDestroyBombWalls items &&
                                         haveItem items Morph &&
                                         canOpenRedDoors items;
            };
            {
                Area = Brinstar;
                Name = "Super Missile (green Brinstar top)";
                Class = Minor;
                Address = 0x7851E;
                Visibility = Visible;
                Available = fun items -> canDestroyBombWalls items &&
                                         haveItem items Morph &&
                                         canOpenRedDoors items;
            };
            {
                Area = Brinstar;
                Name = "Reserve Tank, Brinstar";
                Class = Major;
                Address = 0x7852C;
                Visibility = Chozo;
                Available = fun items -> canDestroyBombWalls items &&
                                         haveItem items Morph &&
                                         canOpenRedDoors items;
            };
            {
                Area = Brinstar;
                Name = "Missile (green Brinstar behind missile)";
                Class = Minor;
                Address = 0x78532;
                Visibility = Hidden;
                Available = fun items -> canDestroyBombWalls items &&
                                         haveItem items Morph &&
                                         canOpenRedDoors items;
            };
            {
                Area = Brinstar;
                Name = "Missile (green Brinstar behind reserve tank)";
                Class = Minor;
                Address = 0x78538;
                Visibility = Visible;
                Available = fun items -> canDestroyBombWalls items &&
                                         haveItem items Morph &&
                                         canOpenRedDoors items;
            };
            {
                Area = Brinstar;
                Name = "Missile (pink Brinstar top)";
                Class = Minor;
                Address = 0x78608;
                Visibility = Visible;
                Available = fun items -> (canDestroyBombWalls items &&
                                            canOpenRedDoors items) ||
                                         canUsePowerBombs items;
            };
            {
                Area = Brinstar;
                Name = "Missile (pink Brinstar bottom)";
                Class = Minor;
                Address = 0x7860E;
                Visibility = Visible;
                Available = fun items -> (canDestroyBombWalls items &&
                                            canOpenRedDoors items) ||
                                         canUsePowerBombs items;
            };
            {
                Area = Brinstar;
                Name = "Charge Beam";
                Class = Major;
                Address = 0x78614;
                Visibility = Chozo;
                Available = fun items -> (canPassBombPassages items &&
                                            canOpenRedDoors items) ||
                                         canUsePowerBombs items;
            };
            {
                Area = Brinstar;
                Name = "Power Bomb (pink Brinstar)";
                Class = Minor;
                Address = 0x7865C;
                Visibility = Visible;
                Available = fun items -> ((canPassBombPassages items &&
                                            canOpenRedDoors items) ||
                                            canUsePowerBombs items) &&
                                            haveItem items Super;
            };
            {
                Area = Brinstar;
                Name = "Missile (green Brinstar pipe)";
                Class = Minor;
                Address = 0x78676;
                Visibility = Visible;
                Available = fun items -> (canDestroyBombWalls items &&
                                            canOpenGreenDoors items) ||
                                         canUsePowerBombs items;
            };
            {
                Area = Brinstar;
                Name = "Morphing Ball";
                Class = Major;
                Address = 0x786DE;
                Visibility = Visible;
                Available = fun items -> true;
            };
            {
                Area = Brinstar;
                Name = "Power Bomb (blue Brinstar)";
                Class = Minor;
                Address = 0x7874C;
                Visibility = Visible;
                Available = fun items -> canUsePowerBombs items;
            };
            {
                Area = Brinstar;
                Name = "Missile (blue Brinstar middle)";
                Address = 0x78798;
                Class = Minor;
                Visibility = Visible;
                Available = fun items -> true;
            };
            {
                Area = Brinstar;
                Name = "Energy Tank, Brinstar Ceiling";
                Class = Major;
                Address = 0x7879E;
                Visibility = Hidden;
                Available = fun items -> true;
            };
            {
                Area = Brinstar;
                Name = "Energy Tank, Etecoons";
                Class = Major;
                Address = 0x787C2;
                Visibility = Visible;
                Available = fun items -> canUsePowerBombs items;
            };
            {
                Area = Brinstar;
                Name = "Super Missile (green Brinstar bottom)";
                Class = Minor;
                Address = 0x787D0;
                Visibility = Visible;
                Available = fun items -> canUsePowerBombs items &&
                                         canOpenGreenDoors items;
            };
            {
                Area = Brinstar;
                Name = "Energy Tank, Waterway";
                Class = Major;
                Address = 0x787FA;
                Visibility = Visible;
                Available = fun items -> canUsePowerBombs items &&
                                         canOpenRedDoors items &&
                                         haveItem items SpeedBooster;
            };
            {
                Area = Brinstar;
                Name = "Missile (blue Brinstar bottom)";
                Class = Minor;
                Address = 0x78802;
                Visibility = Chozo;
                Available = fun items -> haveItem items Morph;
            };
            {
                Area = Brinstar;
                Name = "Energy Tank, Brinstar Gate";
                Class = Major;
                Address = 0x78824;
                Visibility = Visible;
                Available = fun items -> haveItem items Wave &&
                                         ((canPassBombPassages items &&
                                            canOpenRedDoors items) ||
                                                canUsePowerBombs items);
                                    // apparently you can clip thru, no pbs
            };
            {
                Area = Brinstar;
                Name = "Missile (blue Brinstar top)";
                Class = Minor;
                Address = 0x78836;
                Visibility = Visible;
                Available = fun items -> canUsePowerBombs items;
            };
            {
                Area = Brinstar;
                Name = "Missile (blue Brinstar behind missile)";
                Class = Minor;
                Address = 0x7883C;
                Visibility = Hidden;
                Available = fun items -> canUsePowerBombs items;
            };
            {
                Area = Brinstar;
                Name = "X-Ray Scope";
                Class = Major;
                Address = 0x78876;
                Visibility = Chozo;
                Available = fun items ->canAccessRedBrinstar items && 
                                        canUsePowerBombs items;
                                        // bc supers req for red brin also
            };
            {
                Area = Brinstar;
                Name = "Power Bomb (red Brinstar sidehopper room)";
                Class = Minor;
                Address = 0x788CA;
                Visibility = Visible;
                Available = fun items -> canAccessRedBrinstar items &&
                                         canUsePowerBombs items;
                                         // bc supers req for red brin also
            };
            {
                Area = Brinstar;
                Name = "Power Bomb (red Brinstar spike room)";
                Class = Minor;
                Address = 0x7890E;
                Visibility = Chozo;
                Available = fun items -> canAccessRedBrinstar items;
                                         // anti softlock exit thru elevator
            };
            {
                Area = Brinstar;
                Name = "Missile (red Brinstar spike room)";
                Class = Minor;
                Address = 0x78914;
                Visibility = Visible;
                Available = fun items -> canAccessRedBrinstar items &&
                                         canUsePowerBombs items;
            };
            {
                Area = Brinstar;
                Name = "Spazer";
                Class = Major;
                Address = 0x7896E;
                Visibility = Chozo;
                Available = fun items -> canAccessRedBrinstar items &&
                                         canPassBombPassages items;
                                         // supers are included in red brin
            };
            {
                Area = Brinstar;
                Name = "Energy Tank, Kraid";
                Class = Major;
                Address = 0x7899C;
                Visibility = Hidden;
                Available = fun items -> canAccessKraid items;
            };
            {
                Area = Brinstar;
                Name = "Missile (Kraid)";
                Class = Minor;
                Address = 0x789EC;
                Visibility = Hidden;
                Available = fun items -> canAccessKraid items &&
                                         canUsePowerBombs items;
            };
            {
                Area = Brinstar;
                Name = "Varia Suit";
                Class = Major;
                Address = 0x78ACA;
                Visibility = Chozo;
                Available = fun items -> canAccessKraid items;
            };
            {
                Area = Norfair;
                Name = "Missile (lava room)";
                Class = Minor;
                Address = 0x78AE4;
                Visibility = Hidden;
                Available = fun items -> canAccessHeatedNorfair items;
            };
            {
                Area = Norfair;
                Name = "Ice Beam";
                Class = Major;
                Address = 0x78B24;
                Visibility = Chozo;
                Available = fun items -> canAccessRedBrinstar items &&
                                         canPassBombPassages items &&
                                         canHellRunTwo items;
            };
            {
                Area = Norfair;
                Name = "Missile (below Ice Beam)";
                Class = Minor;
                Address = 0x78B46;
                Visibility = Hidden;
                Available = fun items -> canAccessRedBrinstar items &&
                                         canUsePowerBombs items;                                        
            };
            {
                Area = Norfair;
                Name = "Energy Tank, Crocomire";
                Class = Major;
                Address = 0x78BA4;
                Visibility = Visible;
                Available = fun items -> canAccessCrocomire items;
            };
            {
                Area = Norfair;
                Name = "Hi-Jump Boots";
                Class = Major;
                Address = 0x78BAC;
                Visibility = Chozo;
                Available = fun items -> canAccessRedBrinstar items &&
                                         canDestroyBombWalls items;
                                         // supers are included in red brin
            };
            {
                Area = Norfair;
                Name = "Missile (above Crocomire)";
                Class = Minor;
                Address = 0x78BC0;
                Visibility = Visible;
                Available = fun items -> canAccessCrocomire items;
            };
            {
                Area = Norfair;
                Name = "Missile (Hi-Jump Boots)";
                Class = Minor;
                Address = 0x78BE6;
                Visibility = Visible;
                Available = fun items -> canAccessRedBrinstar items;
                                          // supers and a way in are included in red brin
            };
            {
                Area = Norfair;
                Name = "Energy Tank (Hi-Jump Boots)";
                Class = Minor;
                Address = 0x78BEC;
                Visibility = Visible;
                Available = fun items -> canAccessRedBrinstar items;
                                         // supers are included in red brin
                                         // also, some way of dealing with the critter
                                         // whether by bombs, screw, or pbs
            };
            {
                Area = Norfair;
                Name = "Power Bomb (Crocomire)";
                Class = Minor;
                Address = 0x78C04;
                Visibility = Visible;
                Available = fun items -> canAccessCrocomire items;
                                         // supers are included
            };
            {
                Area = Norfair;
                Name = "Missile (below Crocomire)";
                Class = Minor;
                Address = 0x78C14;
                Visibility = Visible;
                Available = fun items -> canAccessCrocomire items;
                                        // check this without speed
            };
            {
                Area = Norfair;
                Name = "Missile (Grapple Beam)";
                Class = Minor;
                Address = 0x78C2A;
                Visibility = Visible;
                Available = fun items -> canAccessCrocomire items &&
                                         (haveItem items SpaceJump ||
                                            haveItem items Grapple ||
                                            canUseBombs items);
            };
            {
                Area = Norfair;
                Name = "Grapple Beam";
                Class = Major;
                Address = 0x78C36;
                Visibility = Chozo;
                Available = fun items -> canAccessCrocomire items;
            };
            {
                Area = Norfair;
                Name = "Reserve Tank, Norfair";
                Class = Major;
                Address = 0x78C3E;
                Visibility = Chozo;
                Available = fun items -> canAccessHeatedNorfair items &&
                                         canHellRunSeven items;
                                            //check
            };
            {
                Area = Norfair;
                Name = "Missile (Norfair Reserve Tank)";
                Class = Minor;
                Address = 0x78C44;
                Visibility = Hidden;
                Available = fun items -> canAccessHeatedNorfair items &&
                                         canHellRunSeven items;
                                            //check
            };
            {
                Area = Norfair;
                Name = "Missile (bubble Norfair green door)";
                Class = Minor;
                Address = 0x78C52;
                Visibility = Visible;
                Available = fun items -> canAccessHeatedNorfair items;
            };
            {
                Area = Norfair;
                Name = "Missile (bubble Norfair)";
                Class = Minor;
                Address = 0x78C66;
                Visibility = Visible;
                Available = fun items -> canAccessHeatedNorfair items &&
                                         canPassBombPassages items;
            };
            {
                Area = Norfair;
                Name = "Missile (Speed Booster)";
                Class = Minor;
                Address = 0x78C74;
                Visibility = Hidden;
                Available = fun items -> canAccessHeatedNorfair items &&
                                         canHellRunFive items &&
                                         canPassBombPassages items;
            };
            {
                Area = Norfair;
                Name = "Speed Booster";
                Class = Major;
                Address = 0x78C82;
                Visibility = Chozo;
                Available = fun items -> canAccessHeatedNorfair items &&
                                         canHellRunFive items &&
                                         canPassBombPassages items;
            };
            {
                Area = Norfair;
                Name = "Missile (Wave Beam)";
                Class = Minor;
                Address = 0x78CBC;
                Visibility = Visible;
                Available = fun items -> canAccessHeatedNorfair items &&
                                         canHellRunThree items;
            };
            {
                Area = Norfair;
                Name = "Wave Beam";
                Class = Major;
                Address = 0x78CCA;
                Visibility = Chozo;
                Available = fun items -> canAccessHeatedNorfair items &&
                                         canHellRunSix items;
            };
            {
                Area = LowerNorfair;
                Name = "Missile (Gold Torizo)";
                Class = Minor;
                Address = 0x78E6E;
                Visibility = Visible;
                Available = fun items -> canAccessLowerNorfair items &&
                                         haveItem items SpaceJump;
            };
            {
                Area = LowerNorfair;
                Name = "Super Missile (Gold Torizo)";
                Class = Minor;
                Address = 0x78E74;
                Visibility = Hidden;
                Available = fun items -> canAccessLowerNorfair items;
            };
            {
                Area = LowerNorfair;
                Name = "Missile (Mickey Mouse room)";
                Class = Minor;
                Address = 0x78F30;
                Visibility = Visible;
                Available = fun items -> canAccessLowerNorfair items;
            };
            {
                Area = LowerNorfair;
                Name = "Missile (lower Norfair above fire flea room)";
                Class = Minor;
                Address = 0x78FCA;
                Visibility = Visible;
                Available = fun items -> canPassWorstRoom items;
            };
            {
                Area = LowerNorfair;
                Name = "Power Bomb (lower Norfair above fire flea room)";
                Class = Minor;
                Address = 0x78FD2;
                Visibility = Visible;
                Available = fun items -> canPassWorstRoom items;
            };
            {
                Area = LowerNorfair;
                Name = "Power Bomb (Power Bombs of shame)";
                Class = Minor;
                Address = 0x790C0;
                Visibility = Visible;
                Available = fun items -> canPassWorstRoom items;
            };
            {
                Area = LowerNorfair;
                Name = "Missile (lower Norfair near Wave Beam)";
                Class = Minor;
                Address = 0x79100;
                Visibility = Visible;
                Available = fun items -> canPassWorstRoom items;
            };
            {
                Area = LowerNorfair;
                Name = "Energy Tank, Ridley";
                Class = Major;
                Address = 0x79108;
                Visibility = Hidden;
                Available = fun items -> canPassWorstRoom items &&
                                         (haveItem items Charge ||
                                             itemCount items Super >= 6);
            };
            {
                Area = LowerNorfair;
                Name = "Screw Attack";
                Class = Major;
                Address = 0x79110;
                Visibility = Chozo;
                Available = fun items -> canAccessLowerNorfair items;
            };
            {
                Area = LowerNorfair;
                Name = "Energy Tank, Firefleas";
                Class = Major;
                Address = 0x79184;
                Visibility = Visible;
                Available = fun items -> canPassWorstRoom items;
            };
            {
                Area = WreckedShip;
                Name = "Missile (Wrecked Ship middle)";
                Class = Minor;
                Address = 0x7C265;
                Visibility = Visible;
                Available = fun items -> canAccessWs items;
            };
            {
                Area = WreckedShip;
                Name = "Reserve Tank, Wrecked Ship";
                Class = Major;
                Address = 0x7C2E9;
                Visibility = Chozo;
                Available = fun items -> canPassAttic items &&
                                         haveItem items SpeedBooster;
            };
            {
                Area = WreckedShip;
                Name = "Missile (Gravity Suit)";
                Class = Minor;
                Address = 0x7C2EF;
                Visibility = Visible;
                Available = fun items -> canPassAttic items;
            };
            {
                Area = WreckedShip;
                Name = "Missile (Wrecked Ship top)";
                Class = Minor;
                Address = 0x7C319;
                Visibility = Visible;
                Available = fun items -> canPassAttic items;
            };
            {
                Area = WreckedShip;
                Name = "Energy Tank, Wrecked Ship";
                Class = Major;
                Address = 0x7C337;
                Visibility = Visible;
                Available = fun items -> canDefeatPhantoon items;
            };
            {
                Area = WreckedShip;
                Name = "Super Missile (Wrecked Ship left)";
                Class = Minor;
                Address = 0x7C357;
                Visibility = Visible;
                Available = fun items -> canDefeatPhantoon items;
            };
            {
                Area = WreckedShip;
                Name = "Right Super, Wrecked Ship";
                Class = Major;
                Address = 0x7C365;
                Visibility = Visible;
                Available = fun items -> canDefeatPhantoon items;
            };
            {
                Area = WreckedShip;
                Name = "Gravity Suit";
                Class = Major;
                Address = 0x7C36D;
                Visibility = Chozo;
                Available = fun items -> canPassAttic items;
            };
            {
                Area = Maridia;
                Name = "Missile (green Maridia shinespark)";
                Class = Minor;
                Address = 0x7C437;
                Visibility = Visible;
                Available = fun items -> canAccessOuterMaridia items &&
                                         haveItem items Gravity &&
                                         haveItem items SpeedBooster &&
                                         haveItem items Morph;
                                         //just get it first try!!
            };
            {
                Area = Maridia;
                Name = "Super Missile (green Maridia)";
                Class = Minor;
                Address = 0x7C43D;
                Visibility = Visible;
                Available = fun items -> canAccessOuterMaridia items;
            };
            {
                Area = Maridia;
                Name = "Energy Tank, Mama turtle";
                Class = Major;
                Address = 0x7C47D;
                Visibility = Visible;
                Available = fun items -> canAccessOuterMaridia items;
            };
            {
                Area = Maridia;
                Name = "Missile (green Maridia tatori)";
                Class = Minor;
                Address = 0x7C483;
                Visibility = Hidden;
                Available = fun items -> canAccessOuterMaridia items;
            };
            {
                Area = Maridia;
                Name = "Super Missile (yellow Maridia)";
                Class = Minor;
                Address = 0x7C4AF;
                Visibility = Visible;
                Available = fun items -> canAccessInnerMaridia items;
            };
            {
                Area = Maridia;
                Name = "Missile (yellow Maridia super missile)";
                Class = Minor;
                Address = 0x7C4B5;
                Visibility = Visible;
                Available = fun items -> canAccessInnerMaridia items;
            };
            {
                Area = Maridia;
                Name = "Missile (yellow Maridia false wall)";
                Class = Minor;
                Address = 0x7C533;
                Visibility = Visible;
                Available = fun items -> canAccessInnerMaridia items &&
                                         (haveItem items Ice ||
                                             haveItem items Gravity);
            };
            {
                Area = Maridia;
                Name = "Plasma Beam";
                Class = Major;
                Address = 0x7C559;
                Visibility = Chozo;
                Available = fun items -> canDefeatDraygon items &&
                                         (haveItem items ScrewAttack ||
                                            haveItem items Plasma ||
                                            (haveItem items Charge &&
                                                ((haveItem items Varia &&
                                                    energyReserveCount items >= 5) ||
                                                    (energyReserveCount items >= 10))));
            };
            {
                Area = Maridia;
                Name = "Missile (left Maridia sand pit room)";
                Class = Minor;
                Address = 0x7C5DD;
                Visibility = Visible;
                Available = fun items -> canAccessInnerMaridia items;
            };
            {
                Area = Maridia;
                Name = "Reserve Tank, Maridia";
                Class = Major;
                Address = 0x7C5E3;
                Visibility = Chozo;
                Available = fun items -> canAccessInnerMaridia items;
            };
            {
                Area = Maridia;
                Name = "Missile (right Maridia sand pit room)";
                Class = Minor;
                Address = 0x7C5EB;
                Visibility = Visible;
                Available = fun items -> canAccessInnerMaridia items;
            };
            {
                Area = Maridia;
                Name = "Power Bomb (right Maridia sand pit room)";
                Class = Minor;
                Address = 0x7C5F1;
                Visibility = Visible;
                Available = fun items -> canAccessInnerMaridia items;
            };
            {
                Area = Maridia;
                Name = "Missile (pink Maridia)";
                Address = 0x7C603;
                Class = Minor;
                Visibility = Visible;
                Available = fun items -> canAccessInnerMaridia items &&
                                         haveItem items Gravity &&
                                         haveItem items SpeedBooster;
            };
            {
                Area = Maridia;
                Name = "Super Missile (pink Maridia)";
                Class = Minor;
                Address = 0x7C609;
                Visibility = Visible;
                Available = fun items -> canAccessInnerMaridia items &&
                                         haveItem items Gravity &&
                                         haveItem items SpeedBooster;
            };
            {
                Area = Maridia;
                Name = "Spring Ball";
                Class = Major;
                Address = 0x7C6E5;
                Visibility = Chozo;
                Available = fun items -> canAccessInnerMaridia items &&
                                         haveItem items Grapple &&
                                         (haveItem items HiJump ||
                                            haveItem items Gravity);
                                         // pb implied by inner maridia
                                         // climbing evir rooms needs gravity
            };
            {
                Area = Maridia;
                Name = "Missile (Draygon)";
                Class = Minor;
                Address = 0x7C74D;
                Visibility = Hidden;
                Available = fun items -> canDefeatBotwoon items;
            };
            {
                Area = Maridia;
                Name = "Energy Tank, Botwoon";
                Class = Major;
                Address = 0x7C755;
                Visibility = Visible;
                Available = fun items -> canDefeatBotwoon items;
            };
            {
                Area = Maridia;
                Name = "Space Jump";
                Class = Major;
                Address = 0x7C7A7;
                Visibility = Chozo;
                Available = fun items -> canDefeatDraygon items;
            }
        ];
