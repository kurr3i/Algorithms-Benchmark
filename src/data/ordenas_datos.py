from pathlib import Path
import pandas as pd

carpeta = Path(__file__).parent  # carpeta donde está este script

df = pd.read_csv(carpeta / "mediciones.csv")

df = df[df["experiment"] == "main"]  # el experimento radix_digits es aparte

tabla = df.pivot_table(index=["input_type", "n"], columns="algorithm", values="time_ns", aggfunc="median")
tabla.to_csv(carpeta / "medianas.csv")
print(tabla.to_string())