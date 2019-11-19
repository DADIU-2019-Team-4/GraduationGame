import matplotlib.pyplot as plt
import numpy as np
import seaborn as sb

def main():
	x = []
	y = []
	with open("file.txt","r") as fp:
		line = fp.readline()
		while line:
			ls,rs= line.split(",")
			x.append(float(ls))
			y.append(float(rs))
			line = fp.readline()
	fig,ax = plt.subplots()
	fig.set_size_inches(4,8)
	ax = sb.kdeplot(x,y,shade=True, alpha=1, n_levels = 100)
	ax.collections[0].set_alpha(0)
	plt.show()

		
if __name__ == "__main__":
	main()