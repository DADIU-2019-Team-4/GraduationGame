import matplotlib.pyplot as plt
import numpy as np
import seaborn as sb

def main():
	x = []
	y = []
	z = []
	with open("data.txt","r") as fp:
		line = fp.readline()
		while line:
			ls,rs,v= line.split(",")
			x.append(float(ls))
			y.append(float(rs))
			z.append(float(v[0:2]))
			line = fp.readline()
	colors = np.random.rand(len(x))
	plt.scatter(x, y, s=z, c=colors, alpha=0.5)
	plt.show()

		
if __name__ == "__main__":
	main()