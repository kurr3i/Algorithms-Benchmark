import numpy as np
import pandas as pd
from pathlib import Path

carpeta = Path(__file__).resolve().parent.parent  # carpeta donde está este script
carpeta = carpeta/"data"  # carpeta donde están las mediciones


med = pd.read_csv(carpeta / "medianas.csv", index_col=["input_type", "n"])

# Pendiente de log(t) vs log(n): t ≈ c·n^k  ->  log t = k·log n + log c
exp = {}
for tipo in med.index.levels[0]:
    for alg in med.columns:
        s = med.loc[tipo, alg].dropna()
        exp[(tipo, alg)] = np.polyfit(np.log(s.index), np.log(s.values), 1)[0]

k = pd.Series(exp).unstack(0).round(2)  # filas = algoritmo, columnas = tipo de entrada
k.to_csv(carpeta / "exponentes.csv")
print(k.to_string())

def orden(x):
    return "O(n²)" if x > 1.5 else "O(n) / O(n log n)"

print("\nOrden práctico:")
print(k.map(orden).to_string())