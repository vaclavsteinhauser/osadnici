﻿using WebOsadnici.Models.HerniTridy;

public class Stavba : HerniEntita
{
    internal string Nazev;
    internal int zisk;
    public Stavba(string Nazev,int zisk)
    {
        this.Nazev = Nazev;
        this.zisk = zisk;
    }
    
}