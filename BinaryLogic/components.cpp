#include "components.h"
#include <iostream>

using namespace std;

void cComponent::init() { }
void cComponent::update() { }

void LeaverComp::update() {
	**this->output = this->state;
}

void NegateComp::init() {
	**this->output = true;
}
void NegateComp::update() {
	**this->output = !(**this->input);
}

void AndComp::update() {
	**this->output = (**this->input) && (**(this->input + 1));
}

void LampComp::update() {
	this->state = **this->input;
}

void XorComp::update() {
	**this->output = (**this->input) ^ (**(this->input + 1));
}

void RepeaterComp::update() {
	//////////////////////////////////////
}

void TickerComp::update() {
	//////////////////////////////////////
}

void FlipFlopComp::update() {
	//////////////////////////////////////
}

void TFlipFlopComp::update() {
	//////////////////////////////////////
}

void ButtonComp::update() {
	//////////////////////////////////////
}

void AnchorComp::update() {
	//////////////////////////////////////
}

void PortalComp::update() {
	//////////////////////////////////////
}

void ExtenderComp::update() {
	//////////////////////////////////////
}

void CounterComp::update() {
	//////////////////////////////////////
}