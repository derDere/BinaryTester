#include "components.h"
#include <iostream>

using namespace std;

void cComponent::update() {
	cout << "..." << endl;
}

void LeaverComp::update() {
	cout << "LeaverComp" << endl;
};

void NegateComp::update() {
	cout << "NegateComp" << endl;
};

void AndComp::update() {
	cout << "AndComp" << endl;
};

void LampComp::update() {
	cout << "LampComp" << endl;
};

void XorComp::update() {
	cout << "XorComp" << endl;
};

void RepeaterComp::update() {
	cout << "RepeaterComp" << endl;
};

void TickerComp::update() {
	cout << "TickerComp" << endl;
};

void FlipFlopComp::update() {
	cout << "FlipFlopComp" << endl;
};

void TFlipFlopComp::update() {
	cout << "TFlipFlopComp" << endl;
};

void ButtonComp::update() {
	cout << "ButtonComp" << endl;
};

void AnchorComp::update() {
	cout << "AnchorComp" << endl;
};

void PortalComp::update() {
	cout << "PortalComp" << endl;
};

void ExtenderComp::update() {
	cout << "ExtenderComp" << endl;
};

void CounterComp::update() {
	cout << "CounterComp" << endl;
};