using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Inventory
{
    // 1. Name - string | 2. Amount - int

    private const int max_slots = 12;
    private static Tuple<string, int>[] items = new Tuple<string, int>[max_slots];
    private static Tuple<string, int>[] max_per_stack = {
        new Tuple<string, int>("Pizza", 10),
        new Tuple<string, int>("Steak", 24),
        new Tuple<string, int>("Cake", 4)
    };

    public static void AddItem(string name, int amt) {

        int max = GetMaxPerStack(name);

        for (int i = 0; i < max_slots; i++) {
            if (items[i] == null) {
                if (amt > max) {
                    items[i] = new Tuple<string, int>(name, max);
                    amt -= max;
                    continue;
                }

                items[i] = new Tuple<string, int>(name, amt);
                break;
            }

            if (items[i].Item1 == name) {
                int slot_space = max - items[i].Item2;

                if (amt > slot_space) {
                    items[i] = new Tuple<string, int>(name, max);
                    amt -= slot_space;
                    continue;
                }

                items[i] = new Tuple<string, int>(name, items[i].Item2 + amt);
                break;
            }
        }
    }

    public static void RemoveItem(string name, int amt) {

        if (GetItemAmount(name) < amt) {
            Debug.Log("Insufficient item amount.");
            return;
        }

        for (int i = max_slots-1; i >= 0; i--) {
            if (items[i] == null)
                continue;
            
            if (items[i].Item1 == name) {
                if (items[i].Item2 > amt) {
                    items[i] = new Tuple<string, int>(name, items[i].Item2-amt);
                    return;
                }

                if (items[i].Item2 == amt) {
                    items[i] = null;
                    return;
                }

                amt -= items[i].Item2;
                items[i] = null;
            }
        }
    }

    public static int GetItemAmount(string name) {

        int count = 0;

        foreach (Tuple<string, int> item in items) {
            if (item == null)
                continue;

            if (item.Item1 == name)
                count += item.Item2;
        }

        return count;
    }

    public static int GetMaxPerStack(string name) {

        foreach (Tuple<string, int> stack in max_per_stack) {
            if (stack.Item1 == name)
                return stack.Item2;
        }
        
        return -1;
    }

    public static void PrintInv() {

        foreach (Tuple<string, int> item in items) {
            if (item == null)
                continue;

            Debug.Log(item.Item1 + " | " + item.Item2);
        }
    }
}
