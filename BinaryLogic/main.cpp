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

	char* path = "C:\\Users\\deremer\\Documents\\TempWorking\\bt\\UnnamedTest.bil";

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
			if (*(dataPoints + con->from))
				*(newStates + con->to) = true;
		}
		for (unsigned short i = 0; i < dataPointCount; i++) {
			*(dataPoints + i) = *(newStates + i);
		}

		for (cComponent* com = compStart; com != NULL; com = com->next) {
			com->update();
		}

		//cout << endl << endl << endl << endl << endl << endl << endl << endl;

		for (cComponent* com = compStart; com != NULL; com = com->next) {
			if (((LeaverComp*)com)->type == LEAVER) {
				//cout << "Leaver: " << ((LeaverComp*)com)->state << endl;
			}
			if (((LampComp*)com)->type == LAMP) {
				//cout << "Lamp: " << ((LampComp*)com)->state << endl;
			}
		}

		//this_thread::sleep_for(100ms);
	}

	cin.ignore();

	return 0;
}

void readComp(ifstream* fs, cComponent* comp) {
	char c, c1, c2;
	unsigned short s;

	while (fs->read(&c, 1) && c == NEXT_PARAM) {
		fs->read(&c1, 1);
		fs->read(&c2, 1);
		s = (c2 << 8) | c1;

		cout << "   i:" << s << endl;

		comp->inputCount += 1;

		comp->input = (bool**)realloc(comp->input, comp->inputCount * sizeof(bool*));

		*(comp->input + (comp->inputCount - 1)) = dataPoints + s;
	}

	while (fs->read(&c, 1) && c == NEXT_PARAM) {
		fs->read(&c1, 1);
		fs->read(&c2, 1);
		s = (c2 << 8) | c1;

		cout << "   o:" << s << endl;

		comp->outputCount += 1;

		comp->output = (bool**)realloc(comp->output, comp->outputCount * sizeof(bool*));

		*(comp->output + (comp->outputCount - 1)) = dataPoints + s;
	}

	comp->init();
};

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
				newComp = new LeaverComp();
				cout << "LEAVER" << endl;
				readComp(&fs, newComp);
				setNextComp(newComp);
				break;

			case NEGATE:
				newComp = new NegateComp();
				cout << "NEGATE" << endl;
				readComp(&fs, newComp);
				setNextComp(newComp);
				break;

			case AND:
				newComp = new AndComp();
				cout << "AND" << endl;
				readComp(&fs, newComp);
				setNextComp(newComp);
				break;

			case LAMP:
				newComp = new LampComp();
				cout << "LAMP" << endl;
				readComp(&fs, newComp);
				setNextComp(newComp);
				break;

			case XOR:
				newComp = new XorComp();
				cout << "XOR" << endl;
				readComp(&fs, newComp);
				setNextComp(newComp);
				break;

			case REPEATER:
				newComp = new RepeaterComp();
				cout << "REPEATER" << endl;
				readComp(&fs, newComp);
				setNextComp(newComp);
				break;

			case TICKER:
				newComp = new TickerComp();
				cout << "TICKER" << endl;
				readComp(&fs, newComp);
				setNextComp(newComp);
				break;

			case FLIP_FLOP:
				newComp = new FlipFlopComp();
				cout << "FLIP_FLOP" << endl;
				readComp(&fs, newComp);
				setNextComp(newComp);
				break;

			case T_FLIP_FLOP:
				newComp = new TFlipFlopComp();
				cout << "T_FLIP_FLOP" << endl;
				readComp(&fs, newComp);
				setNextComp(newComp);
				break;

			case BUTTON:
				newComp = new ButtonComp();
				cout << "BUTTON" << endl;
				readComp(&fs, newComp);
				setNextComp(newComp);
				break;

			case ANCHOR:
				newComp = new AnchorComp();
				cout << "ANCHOR" << endl;
				readComp(&fs, newComp);
				setNextComp(newComp);
				break;

			case PORTAL:
				newComp = new PortalComp();
				cout << "PORTAL" << endl;
				readComp(&fs, newComp);
				setNextComp(newComp);
				break;

			case EXTENDER:
				newComp = new ExtenderComp();
				cout << "EXTENDER" << endl;
				readComp(&fs, newComp);
				setNextComp(newComp);
				break;

			case COUNTER:
				newComp = new CounterComp();
				cout << "COUNTER" << endl;
				readComp(&fs, newComp);
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