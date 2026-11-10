import pandas as pd
import matplotlib.pyplot as plt
from pathlib import Path

carpeta = Path(__file__).resolve().parent.parent  # carpeta donde está este script
carpeta = carpeta/"data"  # carpeta donde están las mediciones

# Archivos generados por tablas.py
med = pd.read_csv(carpeta / "medianas.csv", index_col=["input_type", "n"])
raz = pd.read_csv(carpeta / "razones.csv", index_col=["input_type", "n"])

# 1) Todos los algoritmos con entrada aleatoria, escala log-log
med.loc["Random"].plot(marker="o", logx=True, logy=True, title="Entrada aleatoria (log-log)")
plt.xlabel("n")
plt.ylabel("Tiempo (ns)")
plt.grid(True, which="both", alpha=0.3)
plt.savefig(carpeta / "grafica_random.png", dpi=150)

# 2) Tipos de entrada para Insertion y Quicksort
fig, ejes = plt.subplots(1, 2, figsize=(12, 5))
for eje, alg in zip(ejes, ["Insertion", "Quicksort"]):
    med[alg].unstack(level=0).plot(ax=eje, marker="o", logx=True, logy=True, title=alg)
    eje.set_xlabel("n")
    eje.set_ylabel("Tiempo (ns)")
    eje.grid(True, which="both", alpha=0.3)
plt.tight_layout()
plt.savefig(carpeta / "grafica_tipos_entrada.png", dpi=150)

# 3) Razón t(2n)/t(n) con entrada aleatoria (referencias en 2 y 4)
raz.loc["Random"].plot(marker="o", logx=True, title="Razón t(2n)/t(n) - entrada aleatoria")
plt.axhline(2, color="gray", linestyle="--")
plt.axhline(4, color="gray", linestyle="--")
plt.xlabel("n")
plt.ylabel("t(2n) / t(n)")
plt.grid(True, alpha=0.3)
plt.savefig(carpeta / "grafica_razones.png", dpi=150)

plt.show()