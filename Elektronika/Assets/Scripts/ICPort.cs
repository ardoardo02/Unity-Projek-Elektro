using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICPort : Port
{
    [Header("Connection Settings")]
    [SerializeField, Tooltip("Connected Line Renderer")] string information; // Informasi untuk tipe IC

    public string Information { get => information; }

    // Logika khusus untuk tipe IC dapat ditambahkan di sini
}

