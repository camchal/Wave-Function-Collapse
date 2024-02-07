namespace hamsterbyte.WFC{
	using System;
	using System.Linq;
	using System.Runtime.InteropServices;

	public partial class WFCCell{
		private void PrecalculateFrequencies(){
			for( int i = 0; i< rawFrequencies.Length; i++){
				logFrequencies[i] = Math.Log2(rawFrequencies[i]);
			}

			sumOfRawFrequencies = rawFrequencies.Sum();
			sumOfPossibleFrequencies = sumOfRawFrequencies;
			for (int i = 0; i < rawFrequencies.Length; i++){
				sumOfPossibleFrequencyLogFrequencies += Math.Log2(sumOfRawFrequencies) * Math.Log2(rawFrequencies[i]);
			}
		}

		public void RemoveOption(int i){
			Options[i] = false;
			sumOfPossibleFrequencies -= rawFrequencies[i];
			sumOfPossibleFrequencyLogFrequencies -= logFrequencies[i];
		}

		public double Entropy => Math.Log2(sumOfPossibleFrequencies) - sumOfPossibleFrequencyLogFrequencies/sumOfPossibleFrequencies + entropyNoise;


		private int WeightedRandomIndex(){
			int pointer = 0;
			int randomNumToChooseFromPossible = WFCGrid.Random.Next(0, sumOfPossibleFrequencies);
			for(int i = 0; i < Options.Length; i++){
				if(!Options[i]) continue;
				pointer += rawFrequencies[i];
				if(pointer >= randomNumToChooseFromPossible){
					return i;
				}
			}
			//if returns -1 a contradiction occured
			return -1;
		}


		public int Collapse(){
			int weightedRandomIndex = WeightedRandomIndex();
			TileIndex = weightedRandomIndex;
			Collapsed = true;
			for(int i = 0; i < Options.Length; i++){
				Options[i] = i == TileIndex;
			}
			return weightedRandomIndex;
		}
	}
}
