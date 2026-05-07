using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class AddAllTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Equipment",
                table: "Equipment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Deviations",
                table: "Deviations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TechMapSteps",
                table: "TechMapSteps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TechMaps",
                table: "TechMaps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductionSteps",
                table: "ProductionSteps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductionOrders",
                table: "ProductionOrders");

            migrationBuilder.RenameTable(
                name: "Equipment",
                newName: "equipment");

            migrationBuilder.RenameTable(
                name: "Deviations",
                newName: "deviations");

            migrationBuilder.RenameTable(
                name: "TechMapSteps",
                newName: "tech_map_steps");

            migrationBuilder.RenameTable(
                name: "TechMaps",
                newName: "tech_maps");

            migrationBuilder.RenameTable(
                name: "ProductionSteps",
                newName: "production_steps");

            migrationBuilder.RenameTable(
                name: "ProductionOrders",
                newName: "production_orders");

            migrationBuilder.AlterColumn<decimal>(
                name: "MeasuredValue",
                table: "quality_control",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "equipment",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PlannedTempC",
                table: "production_steps",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PlannedPressureBar",
                table: "production_steps",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualTempC",
                table: "production_steps",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualPressureBar",
                table: "production_steps",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OrderNumber",
                table: "production_orders",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_equipment",
                table: "equipment",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_deviations",
                table: "deviations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tech_map_steps",
                table: "tech_map_steps",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tech_maps",
                table: "tech_maps",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_production_steps",
                table: "production_steps",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_production_orders",
                table: "production_orders",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_quality_control_BatchId",
                table: "quality_control",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_production_batches_OrderId",
                table: "production_batches",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_equipment_Code",
                table: "equipment",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_deviations_BatchId",
                table: "deviations",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_tech_map_steps_EquipmentId",
                table: "tech_map_steps",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_production_steps_BatchId",
                table: "production_steps",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_production_orders_OrderNumber",
                table: "production_orders",
                column: "OrderNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_production_orders_RecipeId",
                table: "production_orders",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_deviations_production_batches_BatchId",
                table: "deviations",
                column: "BatchId",
                principalTable: "production_batches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_production_batches_production_orders_OrderId",
                table: "production_batches",
                column: "OrderId",
                principalTable: "production_orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_production_orders_recipes_RecipeId",
                table: "production_orders",
                column: "RecipeId",
                principalTable: "recipes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_production_steps_production_batches_BatchId",
                table: "production_steps",
                column: "BatchId",
                principalTable: "production_batches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_quality_control_production_batches_BatchId",
                table: "quality_control",
                column: "BatchId",
                principalTable: "production_batches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tech_map_steps_equipment_EquipmentId",
                table: "tech_map_steps",
                column: "EquipmentId",
                principalTable: "equipment",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_deviations_production_batches_BatchId",
                table: "deviations");

            migrationBuilder.DropForeignKey(
                name: "FK_production_batches_production_orders_OrderId",
                table: "production_batches");

            migrationBuilder.DropForeignKey(
                name: "FK_production_orders_recipes_RecipeId",
                table: "production_orders");

            migrationBuilder.DropForeignKey(
                name: "FK_production_steps_production_batches_BatchId",
                table: "production_steps");

            migrationBuilder.DropForeignKey(
                name: "FK_quality_control_production_batches_BatchId",
                table: "quality_control");

            migrationBuilder.DropForeignKey(
                name: "FK_tech_map_steps_equipment_EquipmentId",
                table: "tech_map_steps");

            migrationBuilder.DropIndex(
                name: "IX_quality_control_BatchId",
                table: "quality_control");

            migrationBuilder.DropIndex(
                name: "IX_production_batches_OrderId",
                table: "production_batches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_equipment",
                table: "equipment");

            migrationBuilder.DropIndex(
                name: "IX_equipment_Code",
                table: "equipment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_deviations",
                table: "deviations");

            migrationBuilder.DropIndex(
                name: "IX_deviations_BatchId",
                table: "deviations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tech_maps",
                table: "tech_maps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tech_map_steps",
                table: "tech_map_steps");

            migrationBuilder.DropIndex(
                name: "IX_tech_map_steps_EquipmentId",
                table: "tech_map_steps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_production_steps",
                table: "production_steps");

            migrationBuilder.DropIndex(
                name: "IX_production_steps_BatchId",
                table: "production_steps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_production_orders",
                table: "production_orders");

            migrationBuilder.DropIndex(
                name: "IX_production_orders_OrderNumber",
                table: "production_orders");

            migrationBuilder.DropIndex(
                name: "IX_production_orders_RecipeId",
                table: "production_orders");

            migrationBuilder.RenameTable(
                name: "equipment",
                newName: "Equipment");

            migrationBuilder.RenameTable(
                name: "deviations",
                newName: "Deviations");

            migrationBuilder.RenameTable(
                name: "tech_maps",
                newName: "TechMaps");

            migrationBuilder.RenameTable(
                name: "tech_map_steps",
                newName: "TechMapSteps");

            migrationBuilder.RenameTable(
                name: "production_steps",
                newName: "ProductionSteps");

            migrationBuilder.RenameTable(
                name: "production_orders",
                newName: "ProductionOrders");

            migrationBuilder.AlterColumn<decimal>(
                name: "MeasuredValue",
                table: "quality_control",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldPrecision: 18,
                oldScale: 4,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Equipment",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PlannedTempC",
                table: "ProductionSteps",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PlannedPressureBar",
                table: "ProductionSteps",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualTempC",
                table: "ProductionSteps",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualPressureBar",
                table: "ProductionSteps",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OrderNumber",
                table: "ProductionOrders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Equipment",
                table: "Equipment",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Deviations",
                table: "Deviations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TechMaps",
                table: "TechMaps",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TechMapSteps",
                table: "TechMapSteps",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductionSteps",
                table: "ProductionSteps",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductionOrders",
                table: "ProductionOrders",
                column: "Id");
        }
    }
}
