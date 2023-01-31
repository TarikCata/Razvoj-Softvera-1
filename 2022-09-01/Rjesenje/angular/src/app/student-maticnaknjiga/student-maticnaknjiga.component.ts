import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MojConfig } from '../moj-config';

declare function porukaSuccess(a: string): any;
declare function porukaError(a: string): any;

@Component({
  selector: 'app-student-maticnaknjiga',
  templateUrl: './student-maticnaknjiga.component.html',
  styleUrls: ['./student-maticnaknjiga.component.css'],
})
export class StudentMaticnaknjigaComponent implements OnInit {
  currentStudent: any;
  sveGodine: any;
  akademskeGodine: any;
  datum = new Date().toISOString().slice(0, 10);
  isOpen = false;
  godinaStudija: number;
  cijena: number;
  obnova: boolean;
  akademskaId: number;
  napomena: string;
  isOvjera = false;
  headerTitle: string;
  selectedGodinaId: number;

  constructor(private route: ActivatedRoute, private http: HttpClient) {}

  ngOnInit() {
    let id;
    this.route.paramMap.subscribe((map) => {
      id = map.get('id');
    });
    if (!!id && typeof id != 'number') {
      this.getStudent(id);
      this.getGodine(id);
      this.getAkademske();
    }
  }

  getStudent = (id: number) => {
    const url = `${MojConfig.adresa_servera}/Student/Get/${id}`;
    this.http.get(url, MojConfig.http_opcije()).subscribe((res: any) => {
      if (!!res) {
        const { id, ime, prezime } = res;
        this.currentStudent = { id, ime, prezime };
      } else porukaError(`No student with ${id} id`);
    });
  };

  getGodine = (id: number) => {
    const url = `${MojConfig.adresa_servera}/Godina/GetAll/${id}`;
    this.http.get(url, MojConfig.http_opcije()).subscribe((res: any) => {
      if (!!res) this.sveGodine = res;
      else porukaError(`Nema godina`);
    });
  };

  getAkademske = () => {
    const url = `${MojConfig.adresa_servera}/AkademskeGodine/GetAll_ForCmb`;
    this.http.get(url, MojConfig.http_opcije()).subscribe((res: any) => {
      if (!!res) this.akademskeGodine = res;
      else porukaError(`Nema godina`);
    });
  };

  saveChanges = () => (this.isOvjera ? this.ovjeri() : this.saveGodina());

  saveGodina = () => {
    const url = `${MojConfig.adresa_servera}/Godina/Add`;
    const { id } = this.currentStudent;
    const body = {
      godinaStudija: this.godinaStudija,
      cijena: this.cijena,
      obnova: this.obnova,
      datumUpisa: this.datum,
      akademskaId: this.akademskaId,
      studentId: id,
    };
    const res = this.http
      .post(url, { ...body }, MojConfig.http_opcije())
      .toPromise();
    res
      .then((data) => {
        if (!!data) {
          this.isOpen = false;
        }
      })
      .catch((err) => porukaError(err.error))
      .finally(() => {
        this.getGodine(id);
        porukaSuccess('Uspjesno dodana godina!');
      });
  };

  ovjeri = () => {
    const url = `${MojConfig.adresa_servera}/Godina/Ovjera`;
    const body = {
      id: this.selectedGodinaId,
      napomena: this.napomena,
      datumOvjere: this.datum,
    };
    const { id } = this.currentStudent;
    this.http
      .put(url, { ...body }, MojConfig.http_opcije())
      .subscribe((res) => {
        if (!!res) {
          porukaSuccess('Uspjesno ovjerena godina');
          this.getGodine(id);
          this.isOvjera = false;
          this.isOpen = false;
        } else porukaError('Greska kod ovjere');
      });
  };

  openAkademska = () => {
    const { ime, prezime } = this.currentStudent;
    this.headerTitle = `Novi semestar za ${ime} ${prezime}`;
    this.isOpen = !this.isOpen;
    this.isOvjera = false;
  };

  openOvjera = (id: number) => {
    this.isOpen = true;
    this.isOvjera = true;
    this.selectedGodinaId = id;
  };
}
