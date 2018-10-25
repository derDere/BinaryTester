#include <iostream>
#include <fstream>
#include <stdlib.h>
#include <thread>
#include "components.h"


using namespace std;


void readFile(char* path);


sConnection* conStart;
sConnection* conEnd;

cComponent* compStart;
cComponent* compEnd;

bool* dataPoints;
unsigned short dataPointCount;


int main(int argc, char** argv) {

	//char* path = "C:\\Users\\deremer\\Documents\\TempWorking\\bt\\UnnamedTest.bil";
	//char* path = "F:\\Users\\Phillip\\Documents\\BinaryTests\\AndExampleTest.bil";
	char* path = "F:\\Users\\Phillip\\Documents\\BinaryTests\\JennyBeispiel.bil";

	readFile(path);

	cin.ignore();

	sConnection* con = conStart;
	while (con != NULL) {
		cout << con->from << "->" << con->to << endl;
		cout << (bool)*(dataPoints + con->from) << "->" << (bool)*(dataPoints + con->to) << endl;
		con = con->next;
	}

	cin.ignore();

	cComponent* com = compStart;
	while (com != NULL) {
		for (int i = 0; i < com->inputCount; i++) {
			cout << " i:" << *(com->input + i) << " aka " << (bool)**(com->input + i) << endl;
		}
		for (int i = 0; i < com->outputCount; i++) {
			cout << " o:" << *(com->output + i) << " aka " << (bool)**(com->output + i) << endl;
		}
		com = com->next;
	}

	cin.ignore();

	bool* newStates = (bool*)calloc(dataPointCount, sizeof(bool));
	while (true) {
		memset (newStates, false, dataPointCount);
		for (sConnection* con = conStart; con != NULL; con = con->next) {
			bool* from = dataPoints + con->from;
			bool* to = dataPoints + con->to;
			if (*(dataPoints + con->from))
				*(newStates + con->to) = true;
		}
		for (unsigned short i = 0; i < dataPointCount; i++) {
			bool newState = *(newStates + i);
			*(dataPoints + i) = *(newStates + i);
		}

		for (cComponent* com = compStart; com != NULL; com = com->next) {
			com->update();
		}

		cout << endl << endl << endl << endl << endl << endl << endl << endl;

		for (cComponent* com = compStart; com != NULL; com = com->next) {
			if (((LeaverComp*)com)->type == LEAVER) {
				cout << "Leaver: " << ((LeaverComp*)com)->state << endl;
			}
			if (((LampComp*)com)->type == LAMP) {
				cout << "Lamp: " << ((LampComp*)com)->state << endl;
			}
		}

		//this_thread::sleep_for(1000ms);
	}

	cin.ignore();

	return 0;
}


void setNextCon(sConnection* next) {
	if (conEnd == NULL) {
		conStart = next;
		conEnd = next;
	}
	else {
		conEnd->next = next;
		conEnd = next;
	}
}


void setNextComp(cComponent* next) {
	if (compEnd == NULL) {
		compStart = next;
		compEnd = next;
	}
	else {
		compEnd->next = next;
		compEnd = next;
	}
}


void readFile(char* path) {
	ifstream fs(path, ios::binary | ios::in);

	if (fs.is_open()) {
		char c, c1, c2;
		unsigned short s, from, to;
		sConnection* newCon;
		cComponent* newComp;

		while (fs.read(&c, 1)) {
			switch (c)
			{
			case DATA_POINT:
				fs.read(&c1, 1);
				fs.read(&c2, 1);
				s = (c2 << 8) | c1;
				cout << "DPs:" << s << endl;
				dataPointCount = s;
				dataPoints = (bool*)calloc(s, sizeof(bool));
				break;

			case CONNECTION:
				fs.read(&c1, 1);
				fs.read(&c2, 1);
				from = (c2 << 8) | c1;
				fs.read(&c, 1); // NEXT_PARAM
				fs.read(&c1, 1);
				fs.read(&c2, 1);
				to = (c2 << 8) | c1;
				cout << "Con:" << from << "->" << to << endl;
				newCon = new sConnection();
				newCon->from = from;
				newCon->to = to;
				setNextCon(newCon);
				break;

			case LEAVER:
				cout << "LEAVER" << endl;
				newComp = new LeaverComp(&fs, dataPoints);
				setNextComp(newComp);
				break;

			case NEGATE:
				cout << "NEGATE" << endl;
				newComp = new NegateComp(&fs, dataPoints);
				setNextComp(newComp);
				break;

			case AND:
				cout << "AND" << endl;
				newComp = new AndComp(&fs, dataPoints);
				setNextComp(newComp);
				break;

			case LAMP:
				cout << "LAMP" << endl;
				newComp = new LampComp(&fs, dataPoints);
				setNextComp(newComp);
				break;

			case XOR:
				cout << "XOR" << endl;
				newComp = new XorComp(&fs, dataPoints);
				setNextComp(newComp);
				break;

			case REPEATER:
				cout << "REPEATER" << endl;
				newComp = new RepeaterComp(&fs, dataPoints);
				setNextComp(newComp);
				break;

			case TICKER:
				cout << "TICKER" << endl;
				newComp = new TickerComp(&fs, dataPoints);
				setNextComp(newComp);
				break;

			case FLIP_FLOP:
				cout << "FLIP_FLOP" << endl;
				newComp = new FlipFlopComp(&fs, dataPoints);
				setNextComp(newComp);
				break;

			case T_FLIP_FLOP:
				cout << "T_FLIP_FLOP" << endl;
				newComp = new TFlipFlopComp(&fs, dataPoints);
				setNextComp(newComp);
				break;

			case BUTTON:
				cout << "BUTTON" << endl;
				newComp = new ButtonComp(&fs, dataPoints);
				setNextComp(newComp);
				break;

			case ANCHOR:
				cout << "ANCHOR" << endl;
				newComp = new AnchorComp(&fs, dataPoints);
				setNextComp(newComp);
				break;

			case PORTAL:
				cout << "PORTAL" << endl;
				newComp = new PortalComp(&fs, dataPoints);
				setNextComp(newComp);
				break;

			case EXTENDER:
				cout << "EXTENDER" << endl;
				newComp = new ExtenderComp(&fs, dataPoints);
				setNextComp(newComp);
				break;

			case COUNTER:
				cout << "COUNTER" << endl;
				newComp = new CounterComp(&fs, dataPoints);
				setNextComp(newComp);
				break;

			case APP_END:
				cout << "END" << endl;
				break;

			case NEXT_COMMAND:
				cout << "NEXT" << endl;
				break;

			default:
				cout << "..." << s << endl;
				break;
			}
		}
	}
}