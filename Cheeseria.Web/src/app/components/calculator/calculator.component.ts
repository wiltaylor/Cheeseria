import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-calculator',
  templateUrl: './calculator.component.html',
  styleUrls: ['./calculator.component.css']
})
export class CalculatorComponent implements OnInit {

  constructor() { }

  @Input()CostPerKG: number = 0;
  CurrentKG: number = 1;

  ngOnInit(): void {
  }
}
