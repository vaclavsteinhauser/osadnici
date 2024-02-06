﻿using System.Drawing;
using WebOsadnici.Models.HerniTridy;

public class Rozcesti : HerniEntita
{
    //velikost urcuje prumer kruhu
    internal readonly static Size velikost = new Size(2,2);
    internal readonly int poziceX,poziceY;
    internal Hrac? hrac;
    public bool blokovane=false;
    internal Stavba? stavba;
    internal Rozcesti(int poziceX,int poziceY)
    {
        this.poziceX = poziceX;
        this.poziceY = poziceY;
    }
}