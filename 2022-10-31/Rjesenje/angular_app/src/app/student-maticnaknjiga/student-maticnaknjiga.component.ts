import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MojConfig } from '../moj-config';
import { HttpClient } from '@angular/common/http';

declare function porukaSuccess(a: string): any;
declare function porukaError(a: string): any;

@Component({
  selector: 'app-student-maticnaknjiga',
  templateUrl: './student-maticnaknjiga.component.html',
  styleUrls: ['./student-maticnaknjiga.component.css'],
})
export class StudentMaticnaknjigaComponent implements OnInit {
  upisaneGodine: any;
  currentStudent: any;
  isOpen: boolean = false;
  currentDate = new Date().toISOString().slice(0, 10);
  akademskeGodine: any;
  akademska: any;
  isObnova = false;
  godinaStudija: number;
  cijena: number;
  napomena: string;
  headerTitle: string;
  isOvjera: boolean = false;
  selectedGodinaId: number;

  constructor(private httpKlijent: HttpClient, private route: ActivatedRoute) {}

  ngOnInit(): void {
    let id;
    this.route.paramMap.subscribe((paramMap) => {
      id = paramMap.get('id');
    });
    if (!!id && typeof id != 'number') this.getStudent(id);
    this.getGodine();
    this.getAkademskeGodine();
  }

  getAkademskeGodine = () => {
    const url = `${MojConfig.adresa_servera}/AkademskeGodine/GetAll_ForCmb`;
    this.httpKlijent.get(url, MojConfig.http_opcije()).subscribe((res) => {
      if (!!res) this.akademskeGodine = res;
      else porukaError('Greska kod ucitavanja akademskih godina!');
    });
  };

  getStudent = (id: number) => {
    const url = `${MojConfig.adresa_servera}/Student/GetById?id=${id}`;
    this.httpKlijent.get(url, MojConfig.http_opcije()).subscribe((res: any) => {
      if (!!res) {
        const { id, ime, prezime } = res;
        this.currentStudent = { id, ime, prezime };
      } else porukaError('Greska kod ucitavanja studenta!');
    });
  };

  getGodine = () => {
    const url = `${MojConfig.adresa_servera}/UpisaneGodine/GetAll`;
    this.httpKlijent.get(url, MojConfig.http_opcije()).subscribe((res: any) => {
      if (!!res) this.upisaneGodine = res;
      else porukaError('Greska kod ucitavanja godina!');
    });
  };

  saveChanges = () => (this.isOvjera ? this.ovjeri() : this.saveGodinu());

  saveGodinu = () => {
    const url = `${MojConfig.adresa_servera}/UpisaneGodine/Add`;
    const obj = {
      godinaStudija: this.godinaStudija,
      akademskaGodinaId: this.akademska,
      cijenaSkolarine: this.cijena,
      obnova: this.isObnova,
    };
    this.httpKlijent
      .post(url, { ...obj }, MojConfig.http_opcije())
      .subscribe((res) => {
        if (!!res) {
          this.getGodine();
          this.isOpen = false;
          porukaSuccess('Uspjesno dodana godina!');
        } else porukaError('Greska kod dodavanja godine!');
      });
  };

  ovjeri = () => {
    const url = `${MojConfig.adresa_servera}/UpisaneGodine/Ovjeri`;
    const body = {
      id: this.selectedGodinaId,
      napomena: this.napomena,
    };
    this.httpKlijent
      .put(url, { ...body }, MojConfig.http_opcije())
      .subscribe((res) => {
        if (!!res) {
          porukaSuccess('Uspjesno ovjereno!');
          this.getGodine();
          this.selectedGodinaId = 0;
          this.isOvjera = false;
          this.isOpen = false;
        } else porukaError('Greska kod ovjeravanja!');
      });
  };

  openCloseDialog = () => {
    this.headerTitle = `Nova godina za ${this.currentStudent.ime} ${this.currentStudent.prezime}`;
    this.isOvjera = false;
    this.isOpen = !this.isOpen;
  };

  openOvjera = (id: number) => {
    this.selectedGodinaId = id;
    this.headerTitle = 'Ovjera';
    this.isOpen = !this.isOpen;
    this.isOvjera = !this.isOvjera;
  };
}
