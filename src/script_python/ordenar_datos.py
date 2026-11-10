import pandas as pd
from pathlib import Path

carpeta = Path(__file__).resolve().parent.parent  # carpeta donde está este script
carpeta = carpeta/"data"  # carpeta donde están las mediciones

df = pd.read_csv(carpeta / "mediciones.csv")
df = df[df["experiment"] == "main"]  # el experimento radix_digits es aparte

medianas = df.pivot_table(index=["input_type", "n"], columns="algorithm", values="time_ns", aggfunc="median")
razones = medianas / medianas.groupby("input_type").shift(1)  # t(2n) / t(n)

medianas.to_csv(carpeta / "medianas.csv")
razones.to_csv(carpeta / "razones.csv")

print("MEDIANAS (ns)\n", medianas.to_string())
print("\nRAZON t(2n)/t(n)\n", razones.round(2).to_string())