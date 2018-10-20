#include "components.h"
#include <iostream>
#include <fstream>


using namespace std;


#pragma region cComponent_Funktions
cComponent::cComponent(ifstream* fs, bool* dataPoints) {
	char c, c1, c2;
	unsigned short s;
	//Reading Components Input Datapoints
	while (fs->read(&c, 1) && c == NEXT_PARAM) { //Reading Bytes until file ends or until the NextParam Mark is found
		fs->read(&c1, 1); //Reading LowByte of DataPointId
		fs->read(&c2, 1); //Reading HightByte of DataPointId
		s = (c2 << 8) | c1; //Merging Hight and Low Bytes into DataPointId
		cout << "   i:" << s << endl; //Printing Input Datapoint
		this->inputCount += 1; //UpCounting Input Datapoint Count
		this->input = (bool**)realloc(this->input, this->inputCount * sizeof(bool*)); //Enlarging Input List
		*(this->input + (this->inputCount - 1)) = dataPoints + s; //Adding CurrentDatapoint to Input List
	}
	//Reading Components Output Datapoints
	while (fs->read(&c, 1) && c == NEXT_PARAM) { //Reading Bytes until file ends or until the NextParam Mark is found
		fs->read(&c1, 1); //Reading LowByte of DataPointId
		fs->read(&c2, 1); //Reading HightByte of DataPointId
		s = (c2 << 8) | c1;  //Merging Hight and Low Bytes into DataPointId
		cout << "   o:" << s << endl; //Printing Output Datapoint
		this->outputCount += 1; //UpCounting Output Datapoint Count
		this->output = (bool**)realloc(this->output, this->outputCount * sizeof(bool*)); //Enlarging Output List
		*(this->output + (this->outputCount - 1)) = dataPoints + s; //Adding CurrentDatapoint to Output List
	}
}

void cComponent::update() { }
#pragma endregion


#pragma region LeaverComp_Funktions
LeaverComp::LeaverComp(ifstream* fs, bool* dataPoints) :cComponent(fs, dataPoints) {}

void LeaverComp::update() {
	**this->output = this->state;
}
#pragma endregion


#pragma region NegateComp_Funktions
NegateComp::NegateComp(ifstream* fs, bool* dataPoints) :cComponent(fs, dataPoints) {
	**this->output = true;
}

void NegateComp::update() {
	**this->output = !(**this->input);
}
#pragma endregion


#pragma region AndComp_Funktions
AndComp::AndComp(ifstream* fs, bool* dataPoints) :cComponent(fs, dataPoints) {}

void AndComp::update() {
	**this->output = (**this->input) && (**(this->input + 1));
}
#pragma endregion


#pragma region LampComp_Funktions
LampComp::LampComp(ifstream* fs, bool* dataPoints) :cComponent(fs, dataPoints) {}

void LampComp::update() {
	this->state = **this->input;
}
#pragma endregion


#pragma region XorComp_Funktions
XorComp::XorComp(ifstream* fs, bool* dataPoints) :cComponent(fs, dataPoints) {}

void XorComp::update() {
	**this->output = (**this->input) ^ (**(this->input + 1));
}
#pragma endregion


#pragma region RepeaterComp_Funktions
RepeaterComp::RepeaterComp(ifstream* fs, bool* dataPoints) :cComponent(fs, dataPoints) {}

void RepeaterComp::update() {
	//////////////////////////////////////
}
#pragma endregion


#pragma region TickerComp_Funktions
TickerComp::TickerComp(ifstream* fs, bool* dataPoints) :cComponent(fs, dataPoints) {}

void TickerComp::update() {
	//////////////////////////////////////
}
#pragma endregion


#pragma region FlipFlipComp_Funktions
FlipFlopComp::FlipFlopComp(ifstream* fs, bool* dataPoints) :cComponent(fs, dataPoints) {}

void FlipFlopComp::update() {
	//////////////////////////////////////
}
#pragma endregion


#pragma region TFlipFlopComp_Funktions
TFlipFlopComp::TFlipFlopComp(ifstream* fs, bool* dataPoints) :cComponent(fs, dataPoints) {}

void TFlipFlopComp::update() {
	//////////////////////////////////////
}
#pragma endregion


#pragma region ButtonComp_Funktions
ButtonComp::ButtonComp(ifstream* fs, bool* dataPoints) :cComponent(fs, dataPoints) {}

void ButtonComp::update() {
	//////////////////////////////////////
}
#pragma endregion


#pragma region AnchorComp_Funktions
AnchorComp::AnchorComp(ifstream* fs, bool* dataPoints) :cComponent(fs, dataPoints) {}

void AnchorComp::update() {
	//////////////////////////////////////
}
#pragma endregion


#pragma region PortalComp_Funktions
PortalComp::PortalComp(ifstream* fs, bool* dataPoints) :cComponent(fs, dataPoints) {}

void PortalComp::update() {
	//////////////////////////////////////
}
#pragma endregion


#pragma region ExtenderComp_Funktions
ExtenderComp::ExtenderComp(ifstream* fs, bool* dataPoints) :cComponent(fs, dataPoints) {}

void ExtenderComp::update() {
	//////////////////////////////////////
}
#pragma endregion


#pragma region CounterComp_Funktions
CounterComp::CounterComp(ifstream* fs, bool* dataPoints) :cComponent(fs, dataPoints) {}

void CounterComp::update() {
	//////////////////////////////////////
}
#pragma endregion